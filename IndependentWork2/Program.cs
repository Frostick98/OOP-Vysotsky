public partial class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== ПРОЦЕДУРНА ВЕРСІЯ ===");
        decimal proceduralTotal = RunProceduralDemo();

        Console.WriteLine();
        Console.WriteLine("=== ОБ'ЄКТНА ВЕРСІЯ ===");
        decimal objectTotal = RunObjectOrientedDemo();

        Console.WriteLine();
        Console.WriteLine("=== ПОРІВНЯННЯ ===");
        Console.WriteLine($"Процедурна версія: {proceduralTotal:F2} грн");
        Console.WriteLine($"Об'єктна версія:   {objectTotal:F2} грн");
        Console.WriteLine(proceduralTotal == objectTotal
            ? "Результати збігаються."
            : "Результати НЕ збігаються!");
    }
}
