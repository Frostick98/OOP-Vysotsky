namespace IndependentWork11;

/// <summary>
/// Генерація тестових даних. Використовується Random з фіксованим seed,
/// тому дані (і результати пайплайнів) однакові під час кожного запуску.
/// </summary>
public static class DataGenerator
{
    private static readonly (string Category, decimal MinPrice, decimal MaxPrice, string[] Names)[] Catalog =
    {
        ("Електроніка", 300m, 30000m, new[]
        {
            "Смартфон", "Ноутбук", "Планшет", "Навушники", "Монітор", "Клавіатура",
            "Миша", "Роутер", "Повербанк", "Smart-годинник", "Колонка", "Вебкамера"
        }),
        ("Книги", 100m, 900m, new[]
        {
            "Роман", "Детектив", "Фантастика", "Підручник C#", "Довідник SQL", "Енциклопедія",
            "Комікс", "Поезія", "Біографія", "Кулінарна книга", "Посібник з алгоритмів", "Казки"
        }),
        ("Одяг", 150m, 4000m, new[]
        {
            "Футболка", "Джинси", "Куртка", "Светр", "Кросівки", "Шапка",
            "Шарф", "Сорочка", "Шорти", "Рукавиці", "Плащ", "Спортивний костюм"
        }),
        ("Дім", 80m, 12000m, new[]
        {
            "Чайник", "Пилосос", "Лампа", "Подушка", "Килим", "Сковорода",
            "Набір посуду", "Дзеркало", "Годинник настінний", "Штори", "Мікрохвильовка", "Диван"
        }),
        ("Спорт", 100m, 8000m, new[]
        {
            "М'яч", "Гантелі", "Йога-килимок", "Велосипед", "Скакалка", "Рюкзак туристичний",
            "Намет", "Ролики", "Ракетка", "Пляшка для води", "Фітнес-браслет", "Штанга"
        })
    };

    private static readonly string[] Cities =
    {
        "Рівне", "Київ", "Львів", "Одеса", "Харків", "Дніпро", "Луцьк", "Тернопіль"
    };

    private static readonly string[] FirstNames =
    {
        "Олександр", "Марія", "Іван", "Анна", "Андрій", "Олена", "Дмитро", "Наталя", "Сергій", "Ірина"
    };

    private static readonly string[] LastNames =
    {
        "Шевченко", "Коваленко", "Бондаренко", "Ткаченко", "Кравченко", "Мельник", "Лисенко", "Мороз"
    };

    public static List<Product> GenerateProducts(Random random)
    {
        var products = new List<Product>();
        int id = 1;

        foreach (var (category, minPrice, maxPrice, names) in Catalog)
        {
            foreach (string name in names)
            {
                decimal price = Math.Round(minPrice + (decimal)random.NextDouble() * (maxPrice - minPrice), 0);
                products.Add(new Product { Id = id++, Name = name, Category = category, UnitPrice = price });
            }
        }

        return products; // 5 категорій x 12 = 60 продуктів
    }

    public static List<Customer> GenerateCustomers(Random random)
    {
        var customers = new List<Customer>();
        int id = 1;

        foreach (string lastName in LastNames)
        {
            foreach (string firstName in FirstNames)
            {
                customers.Add(new Customer
                {
                    Id = id++,
                    Name = $"{firstName} {lastName}",
                    City = Cities[random.Next(Cities.Length)]
                });
            }
        }

        return customers; // 10 x 8 = 80 клієнтів
    }

    public static List<Sale> GenerateSales(Random random, int count, List<Product> products, List<Customer> customers)
    {
        var start = new DateTime(2024, 1, 1);
        int days = (new DateTime(2025, 12, 31) - start).Days + 1;
        var sales = new List<Sale>(count);

        for (int i = 1; i <= count; i++)
        {
            sales.Add(new Sale
            {
                Id = i,
                ProductId = products[random.Next(products.Count)].Id,
                CustomerId = customers[random.Next(customers.Count)].Id,
                Quantity = random.Next(1, 6),
                SaleDate = start.AddDays(random.Next(days))
            });
        }

        return sales;
    }
}
