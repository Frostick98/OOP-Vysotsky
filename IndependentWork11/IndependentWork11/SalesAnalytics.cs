namespace IndependentWork11;

/// <summary>
/// Аналітичні LINQ-пайплайни над даними «Продажі».
/// Кожен пайплайн повертає матеріалізований результат (ToList), тому вимірювання
/// часу охоплює реальне виконання, а не лише побудову відкладеного запиту.
/// </summary>
public class SalesAnalytics
{
    private readonly IReadOnlyList<Sale> _sales;
    private readonly IReadOnlyList<Product> _products;
    private readonly IReadOnlyList<Customer> _customers;

    public SalesAnalytics(
        IReadOnlyList<Sale> sales,
        IReadOnlyList<Product> products,
        IReadOnlyList<Customer> customers)
    {
        _sales = sales;
        _products = products;
        _customers = customers;
    }

    /// <summary>
    /// Пайплайн 1. Найпопулярніші продукти за категоріями (топ-3 за проданою кількістю).
    /// GroupBy + Sum, Join, GroupBy за категорією, OrderByDescending + Take.
    /// </summary>
    public List<CategoryTopProducts> TopProductsByCategory() =>
        _sales
            // 1-2. Групуємо продажі за продуктом і рахуємо загальну кількість
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
            // 3. Приєднуємо назву та категорію продукту
            .Join(
                _products,
                total => total.ProductId,
                product => product.Id,
                (total, product) => new ProductSales(product.Id, product.Name, product.Category, total.TotalQuantity))
            // 4. Групуємо за категорією
            .GroupBy(ps => ps.Category)
            // 5. Для кожної категорії беремо 3 найпопулярніші продукти
            .Select(g => new CategoryTopProducts(
                g.Key,
                g.OrderByDescending(ps => ps.TotalQuantity)
                 .ThenBy(ps => ps.Name, StringComparer.Ordinal)
                 .Take(3)
                 .ToList()))
            .OrderBy(c => c.Category, StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// Пайплайн 2. Загальний дохід за містами клієнтів.
    /// Join (продажі + продукти), Join (+ клієнти), Select (дохід), GroupBy, Sum, OrderByDescending.
    /// </summary>
    public List<CityRevenue> RevenueByCity() =>
        _sales
            // 1. Продажі + продукти (для UnitPrice)
            .Join(
                _products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new { sale.CustomerId, Revenue = sale.Quantity * product.UnitPrice })
            // 1. + клієнти (для City)
            .Join(
                _customers,
                x => x.CustomerId,
                customer => customer.Id,
                (x, customer) => new { customer.City, x.Revenue })
            // 3-4. Групуємо за містом і рахуємо суму доходу
            .GroupBy(x => x.City)
            .Select(g => new CityRevenue(g.Key, g.Count(), g.Sum(x => x.Revenue)))
            // 5. Сортуємо за доходом за спаданням
            .OrderByDescending(c => c.Revenue)
            .ThenBy(c => c.City, StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// Пайплайн 3. Продажі за роками та місяцями.
    /// Join, GroupBy за складеним ключем (рік, місяць), Sum, OrderBy + ThenBy.
    /// </summary>
    public List<MonthlySales> SalesByYearAndMonth() =>
        _sales
            .Join(
                _products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new { sale.SaleDate, sale.Quantity, Revenue = sale.Quantity * product.UnitPrice })
            // 1. Групуємо за роком і місяцем (анонімний тип як складений ключ)
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            // 2. Загальна кількість одиниць та загальний дохід
            .Select(g => new MonthlySales(g.Key.Year, g.Key.Month, g.Sum(x => x.Quantity), g.Sum(x => x.Revenue)))
            // 3. Сортуємо за роком, потім за місяцем
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();

    /// <summary>
    /// Пайплайн 4 (додатково). Топ-5 клієнтів за витратами та їхня улюблена категорія
    /// (категорія з найбільшою купленою кількістю).
    /// Join, GroupBy (з вкладеним GroupBy), Sum, Count, OrderByDescending, Take, Join.
    /// </summary>
    public List<CustomerSummary> TopCustomersWithFavoriteCategory() =>
        _sales
            .Join(
                _products,
                sale => sale.ProductId,
                product => product.Id,
                (sale, product) => new
                {
                    sale.CustomerId,
                    product.Category,
                    sale.Quantity,
                    Revenue = sale.Quantity * product.UnitPrice
                })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Orders = g.Count(),
                Spent = g.Sum(x => x.Revenue),
                // Вкладений GroupBy: улюблена категорія клієнта
                FavoriteCategory = g
                    .GroupBy(x => x.Category)
                    .OrderByDescending(c => c.Sum(x => x.Quantity))
                    .ThenBy(c => c.Key, StringComparer.Ordinal)
                    .First().Key
            })
            .OrderByDescending(x => x.Spent)
            .ThenBy(x => x.CustomerId)
            .Take(5)
            // Приєднуємо ім'я та місто клієнта
            .Join(
                _customers,
                x => x.CustomerId,
                customer => customer.Id,
                (x, customer) => new CustomerSummary(customer.Name, customer.City, x.Orders, x.Spent, x.FavoriteCategory))
            .ToList();
}
