namespace lab8v2;

/// <summary>
/// Човен.
/// </summary>
public class Boat : Vehicle
{
    public string EngineType { get; }

    public Boat(double speed, string engineType) : base(speed)
    {
        EngineType = engineType;
    }

    public override void Move()
    {
        Console.WriteLine($"Човен пливе водою (двигун: {EngineType}) зі швидкістю {Speed} км/год.");
    }
}
