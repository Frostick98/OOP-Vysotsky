using System.Diagnostics;

namespace InvertedIndexDemo;

/// <summary>Результат вимірювання для одного набору документів і одного слова.</summary>
public record SearchMeasurement(
    int DocumentCount,
    string Word,
    double LinearMicroseconds,
    double IndexMicroseconds,
    double BuildMicroseconds,
    int LinearFound,
    int IndexFound)
{
    public double Speedup => LinearMicroseconds / IndexMicroseconds;

    /// <summary>
    /// Після скількох пошуків індекс «окупає» витрати на побудову
    /// (null, якщо індекс не швидший за лінійний пошук).
    /// </summary>
    public double? BreakEvenQueries =>
        LinearMicroseconds > IndexMicroseconds
            ? BuildMicroseconds / (LinearMicroseconds - IndexMicroseconds)
            : null;
}

public static class PerformanceTest
{
    /// <summary>
    /// Порівнює лінійний пошук та інвертований індекс для одного слова.
    /// Обидва підходи використовують однакову нормалізацію (TextProcessor.Tokenize).
    /// Час одного пошуку обчислюється як середнє за багато повторів після «прогріву» JIT.
    /// </summary>
    public static SearchMeasurement Measure(List<Document> documents, string searchWord)
    {
        string normalizedWord = TextProcessor.Tokenize(searchWord).Single();

        // Побудова індексу (середнє за кілька побудов)
        const int buildRuns = 5;
        InvertedIndex index = BuildIndex(documents); // прогрів
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < buildRuns; i++)
        {
            index = BuildIndex(documents);
        }
        sw.Stop();
        double buildMicroseconds = ToMicroseconds(sw.ElapsedTicks) / buildRuns;

        // Прогрів пошуків
        List<Document> linearResults = LinearSearch(documents, normalizedWord);
        List<Document> indexResults = index.SearchSingleWord(searchWord);

        // 1. Лінійний пошук
        int linearIterations = Math.Clamp(200_000 / documents.Count, 3, 2000);
        sw.Restart();
        for (int i = 0; i < linearIterations; i++)
        {
            linearResults = LinearSearch(documents, normalizedWord);
        }
        sw.Stop();
        double linearMicroseconds = ToMicroseconds(sw.ElapsedTicks) / linearIterations;

        // 2. Інвертований індекс
        const int indexIterations = 1000;
        sw.Restart();
        for (int i = 0; i < indexIterations; i++)
        {
            indexResults = index.SearchSingleWord(searchWord);
        }
        sw.Stop();
        double indexMicroseconds = ToMicroseconds(sw.ElapsedTicks) / indexIterations;

        return new SearchMeasurement(
            documents.Count, searchWord,
            linearMicroseconds, indexMicroseconds, buildMicroseconds,
            linearResults.Count, indexResults.Count);
    }

    /// <summary>
    /// Виводить порівняння для одного слова у форматі з методички, але з усередненням.
    /// </summary>
    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        SearchMeasurement m = Measure(documents, searchWord);

        Console.WriteLine($"Документів: {m.DocumentCount}, слово: \"{searchWord}\"");
        Console.WriteLine($"Linear search: {m.LinearMicroseconds:F2} мкс");
        Console.WriteLine($"Inverted Index: {m.IndexMicroseconds:F2} мкс");
        Console.WriteLine($"Linear found: {m.LinearFound}, Index found: {m.IndexFound}");
    }

    /// <summary>
    /// Прогоняє вимірювання для кількох обсягів і слів та друкує таблицю у форматі Markdown.
    /// </summary>
    public static void RunBenchmark(int[] documentCounts, string[] words)
    {
        Console.WriteLine("| Документів | Слово | Знайдено | Лінійний, мкс | Індекс, мкс | Прискорення | Побудова індексу, мкс | Окупність, запитів |");
        Console.WriteLine("|---:|---|---:|---:|---:|---:|---:|---:|");

        foreach (int count in documentCounts)
        {
            List<Document> documents = TestData.Generate(count);

            foreach (string word in words)
            {
                SearchMeasurement m = Measure(documents, word);

                if (m.LinearFound != m.IndexFound)
                {
                    throw new InvalidOperationException(
                        $"Результати не збігаються для \"{word}\" ({count} документів): " +
                        $"лінійний {m.LinearFound}, індекс {m.IndexFound}.");
                }

                string breakEven = m.BreakEvenQueries is double q ? $"{Math.Ceiling(q):F0}" : "-";

                Console.WriteLine(
                    $"| {count} | {word} | {m.IndexFound} | {m.LinearMicroseconds:F1} | {m.IndexMicroseconds:F2} | " +
                    $"{m.Speedup:F0}x | {m.BuildMicroseconds:F0} | {breakEven} |");
            }
        }
    }

    private static InvertedIndex BuildIndex(List<Document> documents)
    {
        var index = new InvertedIndex();

        foreach (Document doc in documents)
        {
            index.AddDocument(doc);
        }

        return index;
    }

    private static List<Document> LinearSearch(List<Document> documents, string normalizedWord) =>
        documents
            .Where(d => TextProcessor.Tokenize(d.Content).Contains(normalizedWord))
            .ToList();

    private static double ToMicroseconds(long ticks) =>
        ticks * 1_000_000.0 / Stopwatch.Frequency;
}
