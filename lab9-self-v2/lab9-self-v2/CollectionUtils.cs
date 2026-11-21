namespace Lab9SelfV2;

/// <summary>
/// Власні узагальнені утиліти для колекцій, побудовані на делегатах.
/// </summary>
public static class CollectionUtils
{
    /// <summary>
    /// Перетворює кожен елемент колекції за допомогою mapper (аналог LINQ Select).
    /// Виконується відкладено: елементи обчислюються під час перебору результату.
    /// </summary>
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(mapper);

        return MapIterator(collection, mapper);
    }

    private static IEnumerable<TResult> MapIterator<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        foreach (TSource item in collection)
        {
            yield return mapper(item);
        }
    }

    /// <summary>
    /// Знаходить елемент із максимальним значенням ключа.
    /// Якщо кілька елементів мають однаковий максимальний ключ, повертається перший з них.
    /// </summary>
    /// <exception cref="InvalidOperationException">Якщо колекція порожня.</exception>
    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        bool first = true;
        T maxItem = default!;
        TKey maxKey = default!;

        foreach (T item in collection)
        {
            TKey key = keySelector(item);

            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
        {
            throw new InvalidOperationException("Колекція не містить жодного елемента.");
        }

        return maxItem;
    }

    /// <summary>
    /// Повертає елементи, що задовольняють умову (аналог LINQ Where).
    /// Виконується відкладено.
    /// </summary>
    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        return FilterIterator(collection, predicate);
    }

    private static IEnumerable<T> FilterIterator<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        foreach (T item in collection)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Виконує дію для кожного елемента колекції.
    /// </summary>
    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (T item in collection)
        {
            action(item);
        }
    }
}
