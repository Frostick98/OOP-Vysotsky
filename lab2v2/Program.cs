using System.Runtime.CompilerServices;
using Lab2v2;

internal static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        CreateAndUseCars();

        Console.WriteLine();
        Console.WriteLine("=== End of Main, preparing for GC ===");

        // Лише для навчальних цілей: примусовий запуск збирача сміття
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("=== GC finished ===");
    }

    // NoInlining гарантує, що після виходу з методу посилання на об'єкти зникають,
    // і збирач сміття може їх знищити (інакше JIT може тримати локальні змінні живими).
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void CreateAndUseCars()
    {
        Console.WriteLine("=== Creating objects ===");

        // 1) Конструктор за замовчуванням (ланцюг: Car() -> Car(brand, model, year))
        Car car1 = new Car();

        // 2) Параметризований конструктор
        Car car2 = new Car("Toyota", "Camry", 2018);

        // 3) Ще один параметризований об'єкт
        Car car3 = new Car("Skoda", "Octavia", 2022);

        Console.WriteLine("=== Objects created ===");

        Console.WriteLine();
        Console.WriteLine("=== Calling methods ===");
        car1.StartEngine();
        car2.StartEngine();
        car3.StartEngine();

        Console.WriteLine();
        Console.WriteLine("=== Validation demo ===");
        try
        {
            car2.Year = DateTime.Now.Year + 5;
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Помилка валідації: {ex.Message}");
        }
    }
}
