namespace Lab2v2;

/// <summary>
/// Клас Car: приватні поля, властивості з валідацією,
/// перевантажені конструктори, метод та деструктор (фіналізатор).
/// </summary>
public class Car
{
    // Приватні поля
    private string _brand;
    private string _model;
    private int _year;

    // Публічні властивості
    public string Brand
    {
        get => _brand;
        set => _brand = value;
    }

    public string Model
    {
        get => _model;
        set => _model = value;
    }

    public int Year
    {
        get => _year;
        set
        {
            // Валідація: рік не може бути в майбутньому
            if (value > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    value,
                    "Рік випуску не може бути в майбутньому.");
            }

            _year = value;
        }
    }

    // Конструктор за замовчуванням: ланцюговий виклик параметризованого
    public Car() : this("Unknown", "Unknown", 2000)
    {
        Console.WriteLine("  [Car()] Виконано конструктор за замовчуванням.");
    }

    // Параметризований конструктор
    public Car(string brand, string model, int year)
    {
        _brand = brand;
        _model = model;
        Year = year; // через властивість, щоб спрацювала валідація

        Console.WriteLine($"  [Car(brand, model, year)] Створено авто: {Brand} {Model}, {Year} р.");
    }

    // Метод, пов'язаний із класом
    public void StartEngine()
    {
        Console.WriteLine($"Двигун автомобіля {Brand} {Model} ({Year}) запущено. Врр-врр!");
    }

    // Деструктор (фіналізатор)
    ~Car()
    {
        Console.WriteLine($"  [~Car()] Об'єкт знищено: {Brand} {Model} ({Year})");
    }
}
