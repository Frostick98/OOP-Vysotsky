namespace InvertedIndexDemo;

/// <summary>
/// Тестові документи: п'ять базових із завдання та автоматична генерація великих наборів.
/// </summary>
public static class TestData
{
    private static readonly string[] Vocabulary =
    {
        "memory", "thread", "class", "object", "method", "compiler", "runtime", "network", "database", "query",
        "server", "client", "cache", "queue", "stack", "tree", "graph", "sort", "search", "index",
        "token", "stream", "file", "buffer", "pointer", "function", "variable", "interface", "module", "package",
        "testing", "debugging", "deployment", "framework", "library", "syntax", "algorithm", "pattern", "design", "service"
    };

    public static List<Document> CreateBaseDocuments() => new()
    {
        new Document
        {
            Id = 1,
            Title = "Introduction to C#",
            Content = "C# is a modern object-oriented programming language developed by Microsoft."
        },
        new Document
        {
            Id = 2,
            Title = "Java Basics",
            Content = "Java is a popular programming language known for its portability and performance."
        },
        new Document
        {
            Id = 3,
            Title = "Python Overview",
            Content = "Python is a high-level programming language with simple syntax."
        },
        new Document
        {
            Id = 4,
            Title = "C# Advanced Features",
            Content = "C# supports LINQ, async/await, and many modern programming paradigms."
        },
        new Document
        {
            Id = 5,
            Title = "Comparing Languages",
            Content = "C#, Java, and Python are among the most popular programming languages today."
        }
    };

    /// <summary>
    /// Генерує задану кількість документів: кожен складається з одного з п'яти базових текстів
    /// та 10 випадкових слів зі словника. Seed фіксований, тому набір завжди однаковий.
    /// </summary>
    public static List<Document> Generate(int count, int seed = 42)
    {
        List<Document> baseDocuments = CreateBaseDocuments();
        var random = new Random(seed);
        var documents = new List<Document>(count);

        for (int i = 1; i <= count; i++)
        {
            Document template = baseDocuments[(i - 1) % baseDocuments.Count];
            IEnumerable<string> extraWords = Enumerable.Range(0, 10)
                .Select(_ => Vocabulary[random.Next(Vocabulary.Length)]);

            documents.Add(new Document
            {
                Id = i,
                Title = $"{template.Title} #{i}",
                Content = $"{template.Content} {string.Join(' ', extraWords)}."
            });
        }

        return documents;
    }
}
