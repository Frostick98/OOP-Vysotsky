namespace IndependentWork7;

/// <summary>
/// Узагальнений кеш з обмеженою ємністю та політикою витіснення FIFO
/// (First In, First Out): при переповненні видаляється найстаріший елемент.
/// </summary>
/// <typeparam name="TKey">Тип ключа.</typeparam>
/// <typeparam name="TValue">Тип значення.</typeparam>
public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, TValue> _cache;
    private readonly Queue<TKey> _keysOrder;

    /// <summary>
    /// Створює кеш із заданою максимальною ємністю.
    /// </summary>
    /// <param name="capacity">Максимальна кількість елементів (більше нуля).</param>
    /// <exception cref="ArgumentOutOfRangeException">Якщо ємність менша або дорівнює нулю.</exception>
    public Cache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(capacity), "Ємність кешу має бути більшою за нуль.");
        }

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>(capacity);
    }

    /// <summary>
    /// Поточна кількість елементів у кеші.
    /// </summary>
    public int Count => _cache.Count;

    /// <summary>
    /// Додає елемент до кешу. Якщо кеш повний, видаляє найстаріший елемент.
    /// Якщо ключ уже є в кеші, оновлює значення, не змінюючи порядок ключа.
    /// </summary>
    public void Add(TKey key, TValue value)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count >= _capacity)
        {
            TKey oldestKey = _keysOrder.Dequeue();
            _cache.Remove(oldestKey);
        }

        _cache[key] = value;
        _keysOrder.Enqueue(key);
    }

    /// <summary>
    /// Повертає значення за ключем.
    /// </summary>
    /// <exception cref="KeyNotFoundException">Якщо ключ не знайдено.</exception>
    public TValue Get(TKey key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (_cache.TryGetValue(key, out TValue? value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");
    }

    /// <summary>
    /// Перевіряє, чи містить кеш заданий ключ.
    /// </summary>
    public bool ContainsKey(TKey key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        return _cache.ContainsKey(key);
    }
}
