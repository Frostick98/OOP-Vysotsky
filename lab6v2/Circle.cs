namespace Lab6v2;

/// <summary>
/// Похідний клас «Коло».
/// </summary>
public class Circle : Shape
{
    private double _radius;

    public double Radius
    {
        get => _radius;
        set => _radius = value;
    }

    // Виклик конструктора базового класу через base(...)
    public Circle(string color, double radius) : base(color)
    {
        _radius = radius;
    }

    // Перевизначення віртуального методу
    public override double GetArea()
    {
        return Math.PI * _radius * _radius;
    }

    // Власний унікальний метод
    public void Draw()
    {
        Console.WriteLine($"Малюємо коло ({Color}) з радіусом {Radius}.");
    }
}
