using System.Runtime.CompilerServices;

public class Program
{
    public static void Main(string[] args)
    {
        // Сценарій 1: використання using
        Console.WriteLine("=== Сценарій 1: using ===");
        using (DatabaseConnection db = new DatabaseConnection("Server=localhost;Database=shop"))
        {
            db.ExecuteQuery("SELECT * FROM products");
        } // Dispose() викликається автоматично
        Console.WriteLine();

        // Сценарій 2: явний виклик Dispose()
        Console.WriteLine("=== Сценарій 2: явний Dispose() ===");
        DatabaseConnection db2 = new DatabaseConnection("Server=localhost;Database=users");
        db2.ExecuteQuery("SELECT * FROM users");
        db2.Dispose();
        db2.Dispose(); // повторний виклик нічого не робить і не викликає винятку
        db2.ExecuteQuery("SELECT 1"); // з'єднання вже закрито
        Console.WriteLine();

        // Сценарій 3: без Dispose(), ресурс звільняє деструктор
        Console.WriteLine("=== Сценарій 3: деструктор через GC ===");
        CreateWithoutDispose();
        Console.WriteLine("Перед GC.Collect(): посилань на об'єкт немає, але деструктор ще не викликано.");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine("Після GC.Collect() та GC.WaitForPendingFinalizers().");
    }

    // Окремий метод, щоб після виходу з нього на об'єкт не залишилось посилань
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void CreateWithoutDispose()
    {
        DatabaseConnection db3 = new DatabaseConnection("Server=localhost;Database=logs");
        db3.ExecuteQuery("SELECT * FROM logs");
    }
}
