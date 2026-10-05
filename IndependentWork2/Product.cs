public class Product
{
    private string _name;
    private decimal _price;

    public string Name
    {
        get { return _name; }
    }

    public decimal Price
    {
        get { return _price; }
    }

    public Product(string name, decimal price)
    {
        _name = name;
        _price = price;
    }
}
