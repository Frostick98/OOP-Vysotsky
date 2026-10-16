namespace Lab6v2;

/// <summary>
/// Похідний клас «Прямокутник».
/// </summary>
public class Rectangle : Shape
{
    private double _width;
    private double _height;

    public double Width
    {
        get => _width;
        set => _width = value;
    }

    public double Height
    {
        get => _height;
        set => _height = value;
    }

    // Виклик конструктора базового класу через base(...)
    public Rectangle(string color, double width, double height) : base(color)
    {
        _width = width;
        _height = height;
    }

    // Перевизначення віртуального методу
    public override double GetArea()
    {
        return _width * _height;
    }

    // Власний унікальний метод
    public void Draw()
    {
        Console.WriteLine($"Малюємо прямокутник ({Color}) {Width}x{Height}.");
    }

    // Приховування невіртуального методу базового класу (модифікатор new).
    // Це НЕ перевизначення: який метод буде викликано, залежить від типу посилання.
    public new string GetShapeType()
    {
        return "Rectangle (похідний клас)";
    }
}
