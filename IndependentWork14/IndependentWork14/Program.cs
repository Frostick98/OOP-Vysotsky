using IndependentWork14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DemoFileMetadataCache();
DemoTagManager();
DemoSearchIndex();

static void DemoFileMetadataCache()
{
    Console.WriteLine("=== Сценарій 1: FileMetadataCache (Dictionary) ===");
    var cache = new FileMetadataCache();

    cache.Add(new FileMetadata { FileName = "report.docx", Size = 48_213, LastModified = new DateTime(2026, 11, 3, 14, 20, 0), Hash = "a1b2c3" });
    cache.Add(new FileMetadata { FileName = "photo.png", Size = 2_048_000, LastModified = new DateTime(2026, 10, 28, 9, 5, 0), Hash = "d4e5f6" });
    Console.WriteLine($"Додано 2 файли, Count = {cache.Count}");

    bool duplicate = cache.Add(new FileMetadata { FileName = "REPORT.DOCX", Size = 1, Hash = "zzz" });
    Console.WriteLine($"Повторне додавання REPORT.DOCX (регістр не важливий): {duplicate}");

    Console.WriteLine($"Contains(\"photo.png\")  = {cache.Contains("photo.png")}");
    Console.WriteLine($"Get(\"photo.png\")       = {cache.Get("photo.png")}");
    Console.WriteLine($"Get(\"missing.txt\")     = {(cache.Get("missing.txt")?.ToString() ?? "null")}");

    Console.WriteLine($"Remove(\"photo.png\")    = {cache.Remove("photo.png")}");
    Console.WriteLine($"Remove(\"photo.png\")    = {cache.Remove("photo.png")} (вдруге)");
    Console.WriteLine($"Count = {cache.Count}");
    Console.WriteLine();
}

static void DemoTagManager()
{
    Console.WriteLine("=== Сценарій 2: TagManager (Dictionary + HashSet) ===");
    var tags = new TagManager();

    tags.AddTag("article-1", "C#");
    tags.AddTag("article-1", "collections");
    tags.AddTag("article-1", "performance");
    tags.AddTag("article-1", "c#"); // дублікат: інший регістр

    tags.AddTag("article-2", "C#");
    tags.AddTag("article-2", "testing");
    tags.AddTag("article-2", "performance");

    tags.AddTag("article-3", "python");

    Console.WriteLine($"Теги article-1: {string.Join(", ", tags.GetTags("article-1").Order())}");
    Console.WriteLine($"Теги article-2: {string.Join(", ", tags.GetTags("article-2").Order())}");
    Console.WriteLine($"Усі унікальні теги (UnionWith): {string.Join(", ", tags.GetAllUniqueTags().Order())}");
    Console.WriteLine($"Спільні теги 1 і 2 (IntersectWith): {string.Join(", ", tags.FindCommonTags("article-1", "article-2").Order())}");
    Console.WriteLine($"Лише в article-1 (ExceptWith): {string.Join(", ", tags.FindUniqueTags("article-1", "article-2").Order())}");

    tags.RemoveTag("article-1", "performance");
    Console.WriteLine($"Після RemoveTag(article-1, performance) спільні теги: {string.Join(", ", tags.FindCommonTags("article-1", "article-2").Order())}");
    Console.WriteLine();
}

static void DemoSearchIndex()
{
    Console.WriteLine("=== Сценарій 3: SearchIndex (Dictionary<string, HashSet<int>>) ===");
    var index = new SearchIndex();

    index.AddDocument(1, "Dictionary stores key value pairs");
    index.AddDocument(2, "HashSet stores unique values");
    index.AddDocument(3, "Dictionary and HashSet are fast hash based collections");

    Console.WriteLine($"Search(\"stores\")                 -> [{string.Join(", ", index.Search("stores").Order())}]");
    Console.WriteLine($"Search(\"dictionary\")             -> [{string.Join(", ", index.Search("dictionary").Order())}]");
    Console.WriteLine($"SearchMultiple(\"hashset\",\"fast\") -> [{string.Join(", ", index.SearchMultiple("hashset", "fast").Order())}]");
    Console.WriteLine($"SearchMultiple(\"dictionary\",\"unique\") -> [{string.Join(", ", index.SearchMultiple("dictionary", "unique").Order())}]");
    Console.WriteLine($"Search(\"python\")                 -> [{string.Join(", ", index.Search("python").Order())}]");
}
