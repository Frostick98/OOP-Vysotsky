using System.Collections.Generic;

// Позиція кошика: товар + кількість
public class CartItem
{
    private Product _product;
    private int _quantity;

    public Product Product
    {
        get { return _product; }
    }

    public int Quantity
    {
        get { return _quantity; }
    }

    public CartItem(Product product, int quantity)
    {
        _product = product;
        _quantity = quantity;
    }
}

public class Cart
{
    private const decimal DiscountThreshold = 500m;
    private const decimal DiscountRate = 0.10m;

    private List<CartItem> _items;

    public Cart()
    {
        _items = new List<CartItem>();
    }

    public void AddItem(Product product, int quantity)
    {
        _items.Add(new CartItem(product, quantity));
    }

    // Нова операція, яку в процедурній версії робити набагато складніше
    public bool RemoveItem(string productName)
    {
        CartItem? found = _items.Find(i => i.Product.Name == productName);
        if (found == null)
        {
            return false;
        }
        _items.Remove(found);
        return true;
    }

    // Знижка 10% для товарів, ціна яких більша за 500 грн
    public decimal ApplyDiscount(CartItem item)
    {
        decimal lineTotal = item.Product.Price * item.Quantity;
        return item.Product.Price > DiscountThreshold ? lineTotal * DiscountRate : 0m;
    }

    // Підсумок кошика з урахуванням знижок
    public decimal GetTotal()
    {
        decimal total = 0m;
        foreach (CartItem item in _items)
        {
            total += item.Product.Price * item.Quantity - ApplyDiscount(item);
        }
        return total;
    }

    // Виведення сум по кожній позиції
    public void PrintItems()
    {
        foreach (CartItem item in _items)
        {
            decimal lineTotal = item.Product.Price * item.Quantity;
            decimal discount = ApplyDiscount(item);

            Console.Write($"{item.Product.Name}: {item.Product.Price:F2} x {item.Quantity} = {lineTotal:F2} грн");
            if (discount > 0)
            {
                Console.Write($", знижка {discount:F2} грн, до сплати {lineTotal - discount:F2} грн");
            }
            Console.WriteLine();
        }
    }
}
