namespace IndependentWork14;

/// <summary>
/// Метадані файлу, які зберігаються в кеші.
/// </summary>
public class FileMetadata
{
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public string Hash { get; set; } = string.Empty;

    public override string ToString() =>
        $"{FileName} | {Size} байт | {LastModified:yyyy-MM-dd HH:mm} | hash={Hash}";
}

/// <summary>
/// Сценарій 1. Кеш метаданих файлів на основі Dictionary&lt;string, FileMetadata&gt;.
/// Ім'я файлу є ключем, тому пошук метаданих виконується за O(1) у середньому.
/// Порівняння ключів нечутливе до регістру (як імена файлів у Windows).
/// </summary>
public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Кількість записів у кеші.</summary>
    public int Count => _cache.Count;

    /// <summary>
    /// Додає метадані до кешу.
    /// Повертає false, якщо файл із таким іменем уже є (наявний запис не змінюється).
    /// </summary>
    public bool Add(FileMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentException.ThrowIfNullOrWhiteSpace(metadata.FileName);

        return _cache.TryAdd(metadata.FileName, metadata);
    }

    /// <summary>
    /// Повертає метадані за іменем файлу або null, якщо запису немає.
    /// </summary>
    public FileMetadata? Get(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        return _cache.TryGetValue(fileName, out FileMetadata? metadata) ? metadata : null;
    }

    /// <summary>
    /// Видаляє запис за іменем файлу. Повертає true, якщо запис було видалено.
    /// </summary>
    public bool Remove(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        return _cache.Remove(fileName);
    }

    /// <summary>
    /// Перевіряє, чи є в кеші запис для файлу.
    /// </summary>
    public bool Contains(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        return _cache.ContainsKey(fileName);
    }
}
