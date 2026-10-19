namespace lab8v2;

/// <summary>
/// Базовий клас «Транспортний засіб».
/// </summary>
public class Vehicle
{
    public double Speed { get; }

    public Vehicle(double speed)
    {
        Speed = speed;
    }

    /// <summary>
    /// Віртуальний метод: похідні класи перевизначають його власною реалізацією.
    /// </summary>
    public virtual void Move()
    {
        Console.WriteLine($"Транспортний засіб рухається зі швидкістю {Speed} км/год.");
    }
}
