namespace lab12v2;

/// <summary>
/// Узагальнений репозиторій. Тип T обмежений інтерфейсом IEntity.
/// </summary>
public interface IRepository<T> where T : IEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
}
