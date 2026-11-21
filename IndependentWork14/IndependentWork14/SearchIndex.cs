namespace IndependentWork14;

/// <summary>
/// Сценарій 3 (додатковий). Простий інвертований індекс:
/// слово -> множина id документів, де воно зустрічається.
/// </summary>
public class SearchIndex
{
    private readonly Dictionary<string, HashSet<int>> _invertedIndex =
        new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Розбиває текст на слова та додає документ до індексу.</summary>
    public void AddDocument(int docId, string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        foreach (string word in Tokenize(content))
        {
            if (!_invertedIndex.TryGetValue(word, out HashSet<int>? docs))
            {
                docs = new HashSet<int>();
                _invertedIndex[word] = docs;
            }

            docs.Add(docId);
        }
    }

    /// <summary>Усі документи, що містять слово (порожня множина, якщо таких немає).</summary>
    public HashSet<int> Search(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        return _invertedIndex.TryGetValue(word.Trim(), out HashSet<int>? docs)
            ? new HashSet<int>(docs)
            : new HashSet<int>();
    }

    /// <summary>Документи, що містять ВСІ вказані слова (перетин множин).</summary>
    public HashSet<int> SearchMultiple(params string[] words)
    {
        ArgumentNullException.ThrowIfNull(words);

        if (words.Length == 0)
        {
            return new HashSet<int>();
        }

        HashSet<int> result = Search(words[0]);

        foreach (string word in words.Skip(1))
        {
            if (result.Count == 0)
            {
                break;
            }

            result.IntersectWith(Search(word));
        }

        return result;
    }

    private static IEnumerable<string> Tokenize(string content) =>
        content.Split(
            (char[]?)null, // null = розділяти за пробільними символами
            StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.Trim('.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '-'))
            .Where(w => w.Length > 0);
}
