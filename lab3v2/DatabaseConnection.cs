namespace Lab3v2;

/// <summary>
/// Імітує з'єднання з базою даних.
/// Реалізує IDisposable та повний патерн Dispose.
/// </summary>
public class DatabaseConnection : IDisposable
{
    private bool _disposed = false;
    private string _connectionString;
    private bool _isConnected;

    public string ConnectionString => _connectionString;
    public bool IsConnected => _isConnected;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
        _isConnected = true; // "виділяємо" некерований ресурс
        Console.WriteLine($"З'єднання відкрито: {_connectionString}");
    }

    public void ExecuteQuery(string query)
    {
        if (_isConnected)
        {
            Console.WriteLine($"Виконується запит: {query}");
        }
        else
        {
            Console.WriteLine("Запит не виконано: з'єднання закрито");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Звільнення керованих ресурсів
                Console.WriteLine("Звільнення керованих ресурсів");
            }

            // Звільнення некерованих ресурсів
            if (_isConnected)
            {
                Console.WriteLine("Закриття з'єднання з базою даних");
                _isConnected = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DatabaseConnection()
    {
        Console.WriteLine("Викликано деструктор");
        Dispose(false);
    }
}
