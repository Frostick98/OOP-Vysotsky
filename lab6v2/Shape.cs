namespace Lab6v2;

/// <summary>
/// Базовий клас «Фігура».
/// </summary>
public class Shape
{
    // Приватне поле
    private string _color;

    // Публічна властивість
    public string Color
    {
        get => _color;
        set => _color = value;
    }

    // Конструктор базового класу
    public Shape(string color)
    {
        _color = color;
    }

    // Віртуальний метод: може бути перевизначений у похідних класах
    public virtual double GetArea()
    {
        return 0;
    }

    // Невіртуальний метод: у похідному класі Rectangle буде приховано через new
    public string GetShapeType()
    {
        return "Shape (базовий клас)";
    }
}
