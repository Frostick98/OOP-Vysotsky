namespace lab12v2;

/// <summary>
/// Репозиторій, що зберігає сутності в пам'яті (List&lt;T&gt;).
/// Якщо в доданого елемента Id == 0, йому автоматично призначається наступний вільний Id.
/// Для неіснуючого Id методи GetById, Update і Delete кидають KeyNotFoundException.
/// </summary>
public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();
    private int _nextId = 1;

    public T GetById(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item is null)
            throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
        return item;
    }

    public IEnumerable<T> GetAll() => new List<T>(_items);

    public void Add(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        if (item.Id == 0)
        {
            item.Id = _nextId;
        }
        else if (_items.Any(x => x.Id == item.Id))
        {
            throw new ArgumentException($"Елемент з Id = {item.Id} вже існує.", nameof(item));
        }

        _items.Add(item);
        _nextId = Math.Max(_nextId, item.Id) + 1;
    }

    public void Update(T item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var index = _items.FindIndex(x => x.Id == item.Id);
        if (index < 0)
            throw new KeyNotFoundException($"Елемент з Id = {item.Id} не знайдено.");

        _items[index] = item;
    }

    public void Delete(int id)
    {
        var index = _items.FindIndex(x => x.Id == id);
        if (index < 0)
            throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");

        _items.RemoveAt(index);
    }
}
