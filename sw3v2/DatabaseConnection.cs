public class DatabaseConnection : IDisposable
{
    private string _connectionString;
    private bool _isConnected;
    private bool _disposed = false;

    public string ConnectionString
    {
        get { return _connectionString; }
    }

    public bool IsConnected
    {
        get { return _isConnected; }
    }

    // Конструктор "виділяє ресурс" (відкриває з'єднання)
    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
        _isConnected = true;
        Console.WriteLine($"З'єднання відкрито: {_connectionString}");
    }

    // Виконує запит, лише якщо "підключено"
    public void ExecuteQuery(string query)
    {
        if (_isConnected)
        {
            Console.WriteLine($"Виконується запит: {query}");
        }
        else
        {
            Console.WriteLine("Неможливо виконати запит: з'єднання закрито.");
        }
    }

    // Деструктор (фіналізатор)
    ~DatabaseConnection()
    {
        Dispose(false);
    }

    // Публічний метод звільнення ресурсів
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Захищений віртуальний метод патерну Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Тут звільняються керовані ресурси (у цьому класі їх немає).
            // Звертатися до інших керованих об'єктів можна лише тут,
            // але не у виклику з деструктора.
        }

        // Звільнення "некерованого ресурсу": закриваємо з'єднання
        if (_isConnected)
        {
            _isConnected = false;
            Console.WriteLine(disposing
                ? "З'єднання закрито (Dispose)."
                : "З'єднання закрито (деструктор).");
        }

        _disposed = true;
    }
}
