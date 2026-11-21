using System.Diagnostics;
using System.Globalization;
using IndependentWork11;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// ---------- Extract: генерація даних ----------
var random = new Random(12345); // фіксований seed: дані відтворювані
List<Product> products = DataGenerator.GenerateProducts(random);
List<Customer> customers = DataGenerator.GenerateCustomers(random);
List<Sale> sales = DataGenerator.GenerateSales(random, 1000, products, customers);

Console.WriteLine($"Дані: {products.Count} продуктів, {customers.Count} клієнтів, {sales.Count} продажів");
Console.WriteLine();

var analytics = new SalesAnalytics(sales, products, customers);
var timings = new List<(string Name, double ColdMs, double WarmMs)>();

// ---------- Пайплайн 1 ----------
var top = Measure(analytics.TopProductsByCategory);
PrintHeader("Пайплайн 1: найпопулярніші продукти за категоріями (топ-3)", top);
foreach (CategoryTopProducts category in top.Result)
{
    Console.WriteLine($"{category.Category}:");
    int place = 1;
    foreach (ProductSales p in category.TopProducts)
    {
        Console.WriteLine($"  {place++}. {p.Name,-22} {p.TotalQuantity,4} шт.");
    }
}
Console.WriteLine();
timings.Add(("Пайплайн 1: топ-3 продукти за категоріями", top.ColdMs, top.WarmMs));

// ---------- Пайплайн 2 ----------
var city = Measure(analytics.RevenueByCity);
PrintHeader("Пайплайн 2: загальний дохід за містами клієнтів", city);
foreach (CityRevenue c in city.Result)
{
    Console.WriteLine($"  {c.City,-12} {c.SalesCount,4} продажів  {c.Revenue,12:F2} грн");
}
Console.WriteLine();
timings.Add(("Пайплайн 2: дохід за містами", city.ColdMs, city.WarmMs));

// ---------- Пайплайн 3 ----------
var monthly = Measure(analytics.SalesByYearAndMonth);
PrintHeader("Пайплайн 3: продажі за роками та місяцями", monthly);
foreach (MonthlySales m in monthly.Result)
{
    Console.WriteLine($"  {m.Year}-{m.Month:D2}  {m.Units,4} шт.  {m.Revenue,12:F2} грн");
}
Console.WriteLine();
timings.Add(("Пайплайн 3: продажі за місяцями", monthly.ColdMs, monthly.WarmMs));

// ---------- Пайплайн 4 (додатково) ----------
var customersTop = Measure(analytics.TopCustomersWithFavoriteCategory);
PrintHeader("Пайплайн 4 (додатково): топ-5 клієнтів за витратами", customersTop);
foreach (CustomerSummary c in customersTop.Result)
{
    Console.WriteLine($"  {c.Name,-22} {c.City,-10} {c.Orders,3} покупок  {c.Spent,11:F2} грн  улюблена категорія: {c.FavoriteCategory}");
}
Console.WriteLine();
timings.Add(("Пайплайн 4: топ-5 клієнтів", customersTop.ColdMs, customersTop.WarmMs));

// ---------- Перевірка коректності: порівняння з простим циклом ----------
decimal manualTotal = 0;
var priceById = products.ToDictionary(p => p.Id, p => p.UnitPrice);
foreach (Sale sale in sales)
{
    manualTotal += sale.Quantity * priceById[sale.ProductId];
}

decimal totalByCity = city.Result.Sum(c => c.Revenue);
decimal totalByMonth = monthly.Result.Sum(m => m.Revenue);

Console.WriteLine("=== Перевірка коректності (загальний дохід) ===");
Console.WriteLine($"Ручний цикл:        {manualTotal,12:F2} грн");
Console.WriteLine($"Пайплайн 2 (міста): {totalByCity,12:F2} грн");
Console.WriteLine($"Пайплайн 3 (місяці):{totalByMonth,12:F2} грн");
Console.WriteLine($"Збігаються: {manualTotal == totalByCity && manualTotal == totalByMonth}");
Console.WriteLine();

// ---------- Підсумок вимірювань ----------
Console.WriteLine("=== Підсумок вимірювань часу ===");
Console.WriteLine($"{"Пайплайн",-44} {"Перший запуск, мс",18} {"Середнє (200 запусків), мс",28}");
foreach (var (name, coldMs, warmMs) in timings)
{
    Console.WriteLine($"{name,-44} {coldMs,18:F3} {warmMs,28:F3}");
}

// ---------- Допоміжні методи ----------

// Новий Stopwatch для кожного пайплайну. Перший запуск включає JIT-компіляцію,
// середнє за 200 наступних запусків показує усталений час.
static Measurement<T> Measure<T>(Func<T> pipeline)
{
    const int warmRuns = 200;

    var cold = Stopwatch.StartNew();
    T result = pipeline();
    cold.Stop();

    var warm = Stopwatch.StartNew();
    for (int i = 0; i < warmRuns; i++)
    {
        pipeline();
    }
    warm.Stop();

    return new Measurement<T>(result, ToMs(cold.ElapsedTicks), ToMs(warm.ElapsedTicks) / warmRuns);
}

static double ToMs(long ticks) => ticks * 1000.0 / Stopwatch.Frequency;

static void PrintHeader<T>(string title, Measurement<T> m)
{
    Console.WriteLine($"=== {title} ===");
    Console.WriteLine($"Час: перший запуск {m.ColdMs:F3} мс, середній {m.WarmMs:F3} мс");
}

record Measurement<T>(T Result, double ColdMs, double WarmMs);
