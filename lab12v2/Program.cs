using lab12v2;

var repo = new InMemoryRepository<User>();

// Додавання
repo.Add(new User { Username = "oleksandr", Email = "oleksandr@example.com", IsActive = true });
repo.Add(new User { Username = "ivan", Email = "ivan@example.com", IsActive = false });
repo.Add(new User { Username = "maria", Email = "maria@example.com", IsActive = true });

Console.WriteLine("Після додавання:");
foreach (var user in repo.GetAll())
    Console.WriteLine($"  {user}");

// Отримання за Id
var found = repo.GetById(2);
Console.WriteLine($"\nGetById(2): {found}");

// Оновлення
repo.Update(new User { Id = 2, Username = "ivan", Email = "ivan.new@example.com", IsActive = true });
Console.WriteLine($"Після Update: {repo.GetById(2)}");

// Видалення
repo.Delete(1);
Console.WriteLine("\nПісля видалення Id = 1:");
foreach (var user in repo.GetAll())
    Console.WriteLine($"  {user}");

// Неіснуючий елемент
try
{
    repo.GetById(100);
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"\nОчікувана помилка: {ex.Message}");
}
