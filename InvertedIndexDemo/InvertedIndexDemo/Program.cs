using InvertedIndexDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ---------- Завантаження документів ----------
var index = new InvertedIndex();
foreach (Document doc in TestData.CreateBaseDocuments())
{
    index.AddDocument(doc);
}

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words");
Console.WriteLine();

// ---------- Демонстраційні запити ----------
PrintResults(index, "AND", "programming language", index.SearchMultipleWords("programming", "language"));
PrintResults(index, "AND", "c# async", index.SearchMultipleWords("c#", "async"));
PrintResults(index, "OR", "python lisp java", index.SearchAnyWord("python", "lisp", "java"));
PrintResults(index, "SINGLE", "portability", index.SearchSingleWord("Portability!"));
PrintResults(index, "PHRASE", "modern programming", index.SearchPhrase("modern programming"));
PrintResults(index, "AND", "modern programming (для порівняння з фразою)", index.SearchMultipleWords("modern", "programming"));

// ---------- Утиліта RemoveStopWords ----------
var sentence = TextProcessor.Tokenize("The cat is on the mat, and the dog is in the house.");
Console.WriteLine("Tokenize:        " + string.Join(", ", sentence));
Console.WriteLine("RemoveStopWords: " + string.Join(", ", TextProcessor.RemoveStopWords(sentence)));
Console.WriteLine();

// ---------- Порівняння продуктивності ----------
Console.WriteLine("=== Порівняння лінійного пошуку та інвертованого індексу ===");
Console.WriteLine("(час одного пошуку, середнє за багато повторів; результати обох підходів перевіряються на збіг)");
Console.WriteLine();
PerformanceTest.RunBenchmark(
    new[] { 10, 100, 1000, 10000 },
    new[] { "programming", "java", "paradigms", "quokka" });
Console.WriteLine();

// ---------- Інтерактивна демонстрація (пропускається, якщо вхід перенаправлено) ----------
if (!Console.IsInputRedirected)
{
    while (true)
    {
        Console.Write("Enter search query (or 'exit'): ");
        string? query = Console.ReadLine();

        if (query is null || query == "exit")
        {
            break;
        }

        string[] words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine("\n--- Search Results (AND) ---");
        List<Document> results = index.SearchMultipleWords(words);
        PrintDocuments(results);
        Console.WriteLine($"\nFound {results.Count} documents\n");
    }
}

static void PrintResults(InvertedIndex index, string mode, string query, List<Document> results)
{
    Console.WriteLine($"Query ({mode}): {query}");
    PrintDocuments(results);
    Console.WriteLine($"Found {results.Count} documents");
    Console.WriteLine();
}

static void PrintDocuments(List<Document> results)
{
    foreach (Document doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }
}
