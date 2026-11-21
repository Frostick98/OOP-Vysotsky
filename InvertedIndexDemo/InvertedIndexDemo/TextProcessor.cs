using System.Text.RegularExpressions;

namespace InvertedIndexDemo;

/// <summary>
/// Обробка тексту. Одні й ті самі правила нормалізації використовуються
/// і для індексації, і для запитів, і для лінійного пошуку.
/// </summary>
public static class TextProcessor
{
    // Слово = послідовність літер, цифр та символу '#' (щоб "C#" лишалося окремим словом "c#").
    private static readonly Regex WordRegex = new(@"[\p{L}\p{N}#]+", RegexOptions.Compiled);

    private static readonly HashSet<string> StopWords = new()
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    /// <summary>
    /// Розбиває текст на слова, видаляє пунктуацію, переводить у нижній регістр.
    /// Приклад: "Hello, world!" -> ["hello", "world"]
    /// </summary>
    public static IEnumerable<string> Tokenize(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        foreach (Match match in WordRegex.Matches(text.ToLowerInvariant()))
        {
            yield return match.Value;
        }
    }

    /// <summary>
    /// Видаляє стоп-слова (a, an, the, is, are, etc.)
    /// </summary>
    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        return words.Where(w => !StopWords.Contains(w));
    }
}
