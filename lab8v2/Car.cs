namespace lab8v2;

/// <summary>
/// Автомобіль.
/// </summary>
public class Car : Vehicle
{
    public int NumWheels { get; }

    public Car(double speed, int numWheels) : base(speed)
    {
        NumWheels = numWheels;
    }

    public override void Move()
    {
        Console.WriteLine($"Автомобіль їде дорогою на {NumWheels} колесах зі швидкістю {Speed} км/год.");
    }
}
