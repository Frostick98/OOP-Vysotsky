namespace Lab14v2
{
    /// <summary>
    /// Власний клас для демонстрації роботи CollectionHelper&lt;T&gt;.
    /// </summary>
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public string Category { get; }
        public decimal Price { get; }

        public Product(int id, string name, string category, decimal price)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name} ({Category}): {Price:F2} грн";
        }
    }
}
