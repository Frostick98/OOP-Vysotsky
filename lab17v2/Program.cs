// ============================================================================
// Лабораторна робота №11. LINQ: SelectMany, пагінація й custom extensions.
// Дисципліна: Об'єктно-орієнтоване програмування
// Студент: Висоцький Олександр, група ІПЗ-3/1
// Варіант 2. Сценарій: Замовлення та деталі замовлень з продуктами.
//
// Сутності:
//   Order       (Id, OrderDate, List<OrderDetail> Details)
//   OrderDetail (Id, ProductId, Quantity)  + посилання Product (щоб знати ціну)
//   Product     (Id, Name, Price)
//
// Запити варіанту:
//   1. SelectMany: усі замовлені продукти в одному списку (без дублікатів).
//   2. Пагінація: усі замовлення по 2 замовлення на сторінку.
//   3. Custom Extension: TotalOrderAmount() для IEnumerable<OrderDetail>,
//      що обчислює загальну суму замовлення.
// ============================================================================

using System.Globalization;
using System.Text;

namespace Lab17v2;

public record Product(int Id, string Name, decimal Price);

public class OrderDetail
{
    public int Id { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }

    // Навігаційна властивість: потрібна, щоб метод розширення знав ціну товару.
    public Product Product { get; init; } = null!;
}

public class Order
{
    public int Id { get; init; }
    public DateTime OrderDate { get; init; }
    public List<OrderDetail> Details { get; init; } = new();
}

// ----------------------------------------------------------------------------
// Методи розширення
// ----------------------------------------------------------------------------
public static class Extensions
{
    /// <summary>
    /// Загальна сума замовлення: сума (кількість * ціна продукту) за всіма деталями.
    /// </summary>
    public static decimal TotalOrderAmount(this IEnumerable<OrderDetail> details)
        => details.Sum(d => d.Quantity * d.Product.Price);

    /// <summary>
    /// Універсальна пагінація для будь-якої послідовності (сторінки нумеруються з 1).
    /// </summary>
    public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
        => source.Skip((page - 1) * pageSize).Take(pageSize);
}

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var (products, orders) = Seed();
        Console.WriteLine($"Продуктів: {products.Count}, замовлень: {orders.Count}");

        // ------------------------------------------------------------------
        // 1. SelectMany: усі замовлені продукти без дублікатів
        // ------------------------------------------------------------------
        Console.WriteLine("\n=== 1. SelectMany: усі замовлені продукти (без дублікатів) ===");

        // Для порівняння: Select дає колекцію колекцій
        var nested = orders.Select(o => o.Details);   // IEnumerable<List<OrderDetail>>
        Console.WriteLine($"Select повертає {nested.Count()} вкладених списків (по одному на замовлення)");

        // Синтаксис методів
        var orderedProducts = orders
            .SelectMany(o => o.Details)       // розгортання деталей усіх замовлень
            .Select(d => d.Product)
            .DistinctBy(p => p.Id)            // прибираємо дублікати
            .OrderBy(p => p.Id)
            .ToList();

        // Те саме в синтаксисі запитів
        var orderedProductsQuery = (from o in orders
                                    from d in o.Details
                                    select d.Product)
                                   .DistinctBy(p => p.Id)
                                   .OrderBy(p => p.Id)
                                   .ToList();

        Console.WriteLine($"SelectMany: замовлено {orderedProducts.Count} різних продуктів "
                          + $"(з {orders.Sum(o => o.Details.Count)} позицій у замовленнях):");
        foreach (var p in orderedProducts)
            Console.WriteLine($"  {p.Id,2}. {p.Name,-18} {p.Price,8:F2} грн");
        Console.WriteLine($"Обидва синтаксиси дають однаковий результат: "
                          + $"{orderedProducts.SequenceEqual(orderedProductsQuery)}");

        var notOrdered = products.Where(p => orderedProducts.All(op => op.Id != p.Id)).ToList();
        Console.WriteLine("Не замовлено жодного разу: "
                          + (notOrdered.Count == 0 ? "немає" : string.Join(", ", notOrdered.Select(p => p.Name))));

        // ------------------------------------------------------------------
        // 2. Пагінація: по 2 замовлення на сторінку
        // ------------------------------------------------------------------
        Console.WriteLine("\n=== 2. Пагінація: по 2 замовлення на сторінку ===");
        const int pageSize = 2;
        int totalPages = (int)Math.Ceiling(orders.Count / (double)pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            // Skip/Take у чистому вигляді:
            var pageItems = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            Console.WriteLine($"\n--- Сторінка {page} з {totalPages} ---");
            foreach (var o in pageItems)
                Console.WriteLine($"  Замовлення №{o.Id,-3} {o.OrderDate:dd.MM.yyyy}  позицій: {o.Details.Count}");
        }

        // Те саме через власний метод розширення Page (перевірка, сторінка 3)
        var viaExtension = orders.Page(3, pageSize).Select(o => o.Id);
        Console.WriteLine($"\nPage(3, 2) через метод розширення: замовлення {string.Join(", ", viaExtension)}");

        // ------------------------------------------------------------------
        // 3. Custom Extension: TotalOrderAmount()
        // ------------------------------------------------------------------
        Console.WriteLine("\n=== 3. Custom Extension: TotalOrderAmount() ===");
        foreach (var o in orders)
        {
            Console.WriteLine($"  Замовлення №{o.Id,-3} сума: {o.Details.TotalOrderAmount(),10:F2} грн");
            foreach (var d in o.Details)
                Console.WriteLine($"       {d.Product.Name,-18} {d.Quantity} x {d.Product.Price:F2}");
        }

        // Метод розширення працює з будь-якою послідовністю OrderDetail,
        // тому поєднується з SelectMany: сума всіх замовлень одразу.
        decimal all = orders.SelectMany(o => o.Details).TotalOrderAmount();
        Console.WriteLine($"\nЗагальна сума всіх замовлень: {all:F2} грн");

        var biggest = orders.MaxBy(o => o.Details.TotalOrderAmount())!;
        Console.WriteLine($"Найбільше замовлення: №{biggest.Id} на {biggest.Details.TotalOrderAmount():F2} грн");
    }

    // ------------------------------------------------------------------------
    // Тестові дані: 12 продуктів, 10 замовлень (кожне зі вкладеним списком деталей)
    // ------------------------------------------------------------------------
    private static (List<Product>, List<Order>) Seed()
    {
        var products = new List<Product>
        {
            new(1, "Ноутбук", 28500m),
            new(2, "Миша", 650m),
            new(3, "Клавіатура", 1200m),
            new(4, "Монітор", 7800m),
            new(5, "Навушники", 2100m),
            new(6, "Вебкамера", 1450m),
            new(7, "USB-флешка", 320m),
            new(8, "Зовнішній диск", 2900m),
            new(9, "Роутер", 1750m),
            new(10, "Принтер", 5400m),
            new(11, "Колонки", 1900m),
            new(12, "Планшет", 12400m),
        };

        int detailId = 1;
        OrderDetail D(int productId, int qty) => new()
        {
            Id = detailId++,
            ProductId = productId,
            Quantity = qty,
            Product = products.First(p => p.Id == productId),
        };

        var orders = new List<Order>
        {
            new() { Id = 1,  OrderDate = new(2026, 9, 3),  Details = { D(1, 1), D(2, 1) } },
            new() { Id = 2,  OrderDate = new(2026, 9, 7),  Details = { D(3, 2), D(2, 2), D(7, 5) } },
            new() { Id = 3,  OrderDate = new(2026, 9, 12), Details = { D(4, 2) } },
            new() { Id = 4,  OrderDate = new(2026, 9, 18), Details = { D(5, 1), D(6, 1), D(2, 1) } },
            new() { Id = 5,  OrderDate = new(2026, 9, 25), Details = { D(8, 2), D(7, 3) } },
            new() { Id = 6,  OrderDate = new(2026, 10, 2), Details = { D(9, 1), D(3, 1), D(5, 2), D(2, 3) } },
            new() { Id = 7,  OrderDate = new(2026, 10, 9), Details = { D(10, 1) } },
            new() { Id = 8,  OrderDate = new(2026, 10, 15), Details = { D(1, 2), D(4, 1) } },
            new() { Id = 9,  OrderDate = new(2026, 10, 21), Details = { D(7, 10), D(8, 1) } },
            new() { Id = 10, OrderDate = new(2026, 10, 28), Details = { D(5, 1), D(12, 1), D(3, 1) } },
        };

        return (products, orders);
    }
}

// ============================================================================
// ВИСНОВОК:
// - Select повертає колекцію колекцій (по одному списку деталей на замовлення),
//   а SelectMany "розгортає" їх в одну плоску послідовність, до якої можна
//   застосовувати Select, DistinctBy, OrderBy тощо.
// - Пагінація реалізується через Skip((сторінка - 1) * розмір) і Take(розмір);
//   кількість сторінок = Math.Ceiling(кількість / розмір).
// - Метод розширення TotalOrderAmount() додає бізнес-логіку до
//   IEnumerable<OrderDetail> без зміни коду класів, і його можна ланцюжити
//   з LINQ (наприклад, після SelectMany).
// ============================================================================
