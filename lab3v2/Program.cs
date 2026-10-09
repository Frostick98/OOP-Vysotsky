using System.Runtime.CompilerServices;
using System.Text;
using Lab3v2;

Console.OutputEncoding = Encoding.UTF8;

// Сценарій 1: використання оператора using
Console.WriteLine("=== Сценарій 1: using ===");
using (var db = new DatabaseConnection("Server=localhost;Database=Shop"))
{
    db.ExecuteQuery("SELECT * FROM Products");
}
Console.WriteLine();

// Сценарій 2: явний виклик Dispose()
Console.WriteLine("=== Сценарій 2: явний Dispose() ===");
var db2 = new DatabaseConnection("Server=localhost;Database=Users");
db2.ExecuteQuery("SELECT * FROM Users");
db2.Dispose();
db2.ExecuteQuery("SELECT * FROM Users"); // з'єднання вже закрито
Console.WriteLine();

// Сценарій 3: без Dispose(), очищення через деструктор
Console.WriteLine("=== Сценарій 3: без Dispose(), деструктор через GC ===");
CreateWithoutDispose();
GC.Collect();
GC.WaitForPendingFinalizers();
Console.WriteLine("Збирання сміття завершено");

[MethodImpl(MethodImplOptions.NoInlining)]
static void CreateWithoutDispose()
{
    var db3 = new DatabaseConnection("Server=localhost;Database=Logs");
    db3.ExecuteQuery("SELECT * FROM Logs");
    // Dispose() навмисно не викликається
}
