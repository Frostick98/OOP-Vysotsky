namespace lab8v2;

/// <summary>
/// Велосипед.
/// </summary>
public class Bicycle : Vehicle
{
    public bool HasGears { get; }

    public Bicycle(double speed, bool hasGears) : base(speed)
    {
        HasGears = hasGears;
    }

    public override void Move()
    {
        string gears = HasGears ? "перемикає передачі" : "їде на одній передачі";
        Console.WriteLine($"Велосипедист педалює і {gears}, швидкість {Speed} км/год.");
    }
}
