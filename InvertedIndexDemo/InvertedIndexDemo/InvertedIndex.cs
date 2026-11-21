namespace InvertedIndexDemo;

/// <summary>
/// Інвертований індекс: слово -> множина id документів, де воно зустрічається.
/// </summary>
public class InvertedIndex
{
    // Слово -> множина ID документів
    private readonly Dictionary<string, HashSet<int>> _index;

    // ID -> документ (для швидкого отримання)
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    /// <summary>Кількість документів в індексі.</summary>
    public int DocumentCount => _documents.Count;

    /// <summary>Кількість унікальних слів в індексі.</summary>
    public int UniqueWords => _index.Keys.Count;

    /// <summary>
    /// Додає документ до індексу.
    /// </summary>
    /// <exception cref="ArgumentException">Якщо документ з таким Id уже є в індексі.</exception>
    public void AddDocument(Document doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        // 1. Зберегти документ у _documents
        if (!_documents.TryAdd(doc.Id, doc))
        {
            throw new ArgumentException($"Документ з Id = {doc.Id} уже додано.", nameof(doc));
        }

        // 2. Розбити Content на слова (нижній регістр, без пунктуації)
        // 3. Для кожного слова додати doc.Id до _index[word]
        foreach (string word in TextProcessor.Tokenize(doc.Content))
        {
            if (!_index.TryGetValue(word, out HashSet<int>? ids))
            {
                ids = new HashSet<int>();
                _index[word] = ids;
            }

            ids.Add(doc.Id);
        }
    }

    /// <summary>
    /// Шукає документи, що містять слово. Запит нормалізується так само, як і текст документів.
    /// </summary>
    public List<Document> SearchSingleWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        return SearchMultipleWords(word);
    }

    /// <summary>
    /// Шукає документи, що містять ВСІ слова (AND): перетин множин через IntersectWith.
    /// </summary>
    public List<Document> SearchMultipleWords(params string[] words)
    {
        ArgumentNullException.ThrowIfNull(words);

        HashSet<int>? result = null;

        foreach (string word in NormalizeQuery(words))
        {
            if (!_index.TryGetValue(word, out HashSet<int>? ids))
            {
                return new List<Document>(); // слова немає в індексі, отже, спільних документів теж немає
            }

            if (result is null)
            {
                result = new HashSet<int>(ids); // копія, щоб не змінити сам індекс
            }
            else
            {
                result.IntersectWith(ids);

                if (result.Count == 0)
                {
                    break;
                }
            }
        }

        return ToDocuments(result);
    }

    /// <summary>
    /// Шукає документи, що містять БУДЬ-ЯКЕ слово (OR): об'єднання множин через UnionWith.
    /// </summary>
    public List<Document> SearchAnyWord(params string[] words)
    {
        ArgumentNullException.ThrowIfNull(words);

        var result = new HashSet<int>();

        foreach (string word in NormalizeQuery(words))
        {
            if (_index.TryGetValue(word, out HashSet<int>? ids))
            {
                result.UnionWith(ids);
            }
        }

        return ToDocuments(result);
    }

    /// <summary>
    /// Додатково: пошук точної фрази. Кандидати знаходяться через індекс (AND),
    /// а потім відфільтровуються за порядком слів у тексті документа.
    /// </summary>
    public List<Document> SearchPhrase(string phrase)
    {
        ArgumentNullException.ThrowIfNull(phrase);

        string[] phraseWords = TextProcessor.Tokenize(phrase).ToArray();

        if (phraseWords.Length == 0)
        {
            return new List<Document>();
        }

        return SearchMultipleWords(phraseWords)
            .Where(doc => ContainsSequence(TextProcessor.Tokenize(doc.Content).ToArray(), phraseWords))
            .ToList();
    }

    private static IEnumerable<string> NormalizeQuery(IEnumerable<string> words) =>
        words.SelectMany(TextProcessor.Tokenize);

    private List<Document> ToDocuments(HashSet<int>? ids) =>
        ids is null
            ? new List<Document>()
            : ids.Order().Select(id => _documents[id]).ToList();

    private static bool ContainsSequence(string[] text, string[] phrase)
    {
        for (int start = 0; start + phrase.Length <= text.Length; start++)
        {
            bool match = true;

            for (int i = 0; i < phrase.Length; i++)
            {
                if (text[start + i] != phrase[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                return true;
            }
        }

        return false;
    }
}
