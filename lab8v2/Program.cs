using System.Globalization;
using System.Text;

namespace lab8v2;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        // Колекція об'єктів базового типу
        List<Vehicle> vehicles = new List<Vehicle>
        {
            new Car(90, 4),
            new Bicycle(18, true),
            new Boat(35, "дизельний"),
            new Car(120, 4)
        };

        Console.WriteLine("=== Поліморфний виклик Move() ===");
        foreach (Vehicle vehicle in vehicles)
        {
            // Фактична реалізація обирається під час виконання (динамічне зв'язування)
            vehicle.Move();
        }

        // Агрегація: середня швидкість усіх транспортних засобів
        double totalSpeed = 0;
        foreach (Vehicle vehicle in vehicles)
        {
            totalSpeed += vehicle.Speed;
        }
        double averageSpeed = totalSpeed / vehicles.Count;

        Console.WriteLine();
        Console.WriteLine("=== Агрегація результатів ===");
        Console.WriteLine($"Кількість транспортних засобів: {vehicles.Count}");
        Console.WriteLine($"Середня швидкість: {averageSpeed:F2} км/год");
    }
}
