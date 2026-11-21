namespace IndependentWork11;

/// <summary>Продаж: який клієнт, коли і скільки одиниць якого продукту купив.</summary>
public class Sale
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
}

/// <summary>Продукт каталогу.</summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
}

/// <summary>Клієнт.</summary>
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

// ---------- Типи результатів пайплайнів ----------

public record ProductSales(int ProductId, string Name, string Category, int TotalQuantity);

public record CategoryTopProducts(string Category, IReadOnlyList<ProductSales> TopProducts);

public record CityRevenue(string City, int SalesCount, decimal Revenue);

public record MonthlySales(int Year, int Month, int Units, decimal Revenue);

public record CustomerSummary(string Name, string City, int Orders, decimal Spent, string FavoriteCategory);
