namespace IndependentWork14;

/// <summary>
/// Сценарій 2. Система тегів для статей блогу.
/// Dictionary&lt;ArticleId, HashSet&lt;Tag&gt;&gt;: кожна стаття має множину унікальних тегів.
/// Теги порівнюються без урахування регістру ("C#" і "c#" це один тег).
/// </summary>
public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    /// <summary>Додає тег до статті. Повторне додавання того самого тегу нічого не змінює.</summary>
    public void AddTag(string articleId, string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        if (!_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _articleTags[articleId] = tags;
        }

        tags.Add(tag.Trim());
    }

    /// <summary>Видаляє тег зі статті. Якщо в статті не лишилось тегів, запис про неї видаляється.</summary>
    public void RemoveTag(string articleId, string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        if (_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            tags.Remove(tag.Trim());

            if (tags.Count == 0)
            {
                _articleTags.Remove(articleId);
            }
        }
    }

    /// <summary>Повертає копію множини тегів статті (порожню, якщо статті немає).</summary>
    public HashSet<string> GetTags(string articleId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);

        return _articleTags.TryGetValue(articleId, out HashSet<string>? tags)
            ? new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>Усі унікальні теги всіх статей (об'єднання множин, UnionWith).</summary>
    public HashSet<string> GetAllUniqueTags()
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (HashSet<string> tags in _articleTags.Values)
        {
            result.UnionWith(tags);
        }

        return result;
    }

    /// <summary>Теги, спільні для двох статей (перетин множин, IntersectWith).</summary>
    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);
        result.IntersectWith(GetTags(articleId2));
        return result;
    }

    /// <summary>Теги, які є в першій статті, але відсутні в другій (різниця множин, ExceptWith).</summary>
    public HashSet<string> FindUniqueTags(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);
        result.ExceptWith(GetTags(articleId2));
        return result;
    }
}
