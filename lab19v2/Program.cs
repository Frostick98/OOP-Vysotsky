// ============================================================================
// Лабораторна робота №13. Власні винятки та патерн Result.
// Дисципліна: Об'єктно-орієнтоване програмування
// Студент: Висоцький Олександр, група ІПЗ-3/1
// Варіант 2. Сценарій: Управління користувачами.
//
//   Власний виняток: UserNotFoundException (string Username)
//   Service: UserService
//     - User GetUser(string username)                         -> кидає UserNotFoundException
//     - Result<bool> RegisterUser(string username, string password) -> повертає Result
// ============================================================================

using System.Text;

namespace Lab19v2;

// ----------------------------------------------------------------------------
// Власний виняток
// ----------------------------------------------------------------------------
public class UserNotFoundException : Exception
{
    /// <summary>Ім'я користувача, якого не знайдено.</summary>
    public string Username { get; } = string.Empty;

    public UserNotFoundException() : base("Користувача не знайдено") { }

    public UserNotFoundException(string username)
        : base($"Користувача \"{username}\" не знайдено")
    {
        Username = username;
    }

    public UserNotFoundException(string username, Exception innerException)
        : base($"Користувача \"{username}\" не знайдено", innerException)
    {
        Username = username;
    }
}

// ----------------------------------------------------------------------------
// Узагальнений Result<T>
// ----------------------------------------------------------------------------
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(T? value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value, true, string.Empty);

    public static Result<T> Failure(string error) => new(default, false, error);

    // Зручність: перетворення винятку на Result (береться повідомлення винятку)
    public static Result<T> Failure(Exception ex) => new(default, false, ex.Message);
}

// ----------------------------------------------------------------------------
// Модель
// ----------------------------------------------------------------------------
public class User
{
    public string Username { get; }
    public string Password { get; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public override string ToString() => Username;
}

// ----------------------------------------------------------------------------
// Сервіс
// ----------------------------------------------------------------------------
public class UserService
{
    private const int MinPasswordLength = 6;

    private readonly Dictionary<string, User> _users =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["admin"] = new User("admin", "admin123"),
            ["oleksandr"] = new User("oleksandr", "qwerty1"),
            ["maria"] = new User("maria", "pass2026"),
        };

    /// <summary>
    /// Відсутність користувача при прямому зверненні за ім'ям є винятковою
    /// ситуацією (викликач очікує, що користувач існує), тому кидаємо виняток.
    /// </summary>
    public User GetUser(string username)
    {
        if (_users.TryGetValue(username, out var user))
            return user;

        throw new UserNotFoundException(username);
    }

    /// <summary>
    /// Помилки реєстрації (зайняте ім'я, короткий пароль) - очікувані сценарії,
    /// тому повертаємо Result замість винятків.
    /// </summary>
    public Result<bool> RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result<bool>.Failure("Ім'я користувача не може бути порожнім");

        if (_users.ContainsKey(username))
            return Result<bool>.Failure($"Користувач \"{username}\" уже існує");

        if (string.IsNullOrEmpty(password) || password.Length < MinPasswordLength)
            return Result<bool>.Failure($"Пароль занадто короткий (мінімум {MinPasswordLength} символів)");

        _users[username] = new User(username, password);
        return Result<bool>.Success(true);
    }

    public int Count => _users.Count;
}

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var service = new UserService();
        Console.WriteLine($"Користувачів у системі: {service.Count}\n");

        // ---------------- 1. GetUser: виняток UserNotFoundException ----------------
        Console.WriteLine("=== 1. GetUser (власний виняток) ===");
        foreach (var name in new[] { "admin", "MARIA", "petro" })
        {
            try
            {
                var user = service.GetUser(name);
                Console.WriteLine($"  Знайдено: {user}");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine($"  Помилка: {ex.Message} (Username = {ex.Username})");
            }
            finally
            {
                Console.WriteLine($"  Пошук \"{name}\" завершено");
            }
        }

        // ---------------- 2. RegisterUser: Result<bool> ----------------
        Console.WriteLine("\n=== 2. RegisterUser (патерн Result) ===");
        var attempts = new (string Name, string Password)[]
        {
            ("petro", "secret99"),   // успіх
            ("admin", "newpass1"),   // ім'я вже існує
            ("olena", "123"),        // пароль занадто короткий
            ("   ", "validpass"),    // порожнє ім'я
        };

        foreach (var (name, password) in attempts)
        {
            var result = service.RegisterUser(name, password);
            if (result.IsSuccess)
                Console.WriteLine($"  \"{name}\": успішно зареєстровано (Value = {result.Value})");
            else
                Console.WriteLine($"  \"{name}\": ПОМИЛКА, {result.Error}");
        }

        Console.WriteLine($"\nКористувачів після реєстрації: {service.Count}");

        // ---------------- 3. Failure(Exception): перетворення винятку на Result ----------------
        Console.WriteLine("\n=== 3. Result<User>.Failure(Exception) ===");
        Result<User> found;
        try
        {
            found = Result<User>.Success(service.GetUser("petro"));
        }
        catch (UserNotFoundException ex)
        {
            found = Result<User>.Failure(ex);
        }
        Console.WriteLine(found.IsSuccess
            ? $"  Користувача знайдено: {found.Value}"
            : $"  Помилка: {found.Error}");

        try
        {
            found = Result<User>.Success(service.GetUser("ghost"));
        }
        catch (UserNotFoundException ex)
        {
            found = Result<User>.Failure(ex);
        }
        Console.WriteLine(found.IsSuccess
            ? $"  Користувача знайдено: {found.Value}"
            : $"  Помилка: {found.Error}");
    }
}

// ============================================================================
// ВИСНОВОК:
// - GetUser кидає UserNotFoundException: викликач звертається до конкретного
//   користувача й очікує, що той існує, тому відсутність є винятковою
//   ситуацією, яку не можна ігнорувати; виняток несе Username для діагностики.
// - RegisterUser повертає Result<bool>: зайняте ім'я чи короткий пароль -
//   звичайні очікувані помилки введення, їх зручно перевіряти через
//   IsSuccess/Error без try-catch і без витрат на створення винятків.
// ============================================================================
