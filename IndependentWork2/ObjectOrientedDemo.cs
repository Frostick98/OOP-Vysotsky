public partial class Program
{
    public static decimal RunObjectOrientedDemo()
    {
        // Той самий набір товарів, що й у процедурній версії
        Cart cart = new Cart();
        cart.AddItem(new Product("Мишка", 350m), 2);
        cart.AddItem(new Product("Клавіатура", 800m), 1);
        cart.AddItem(new Product("Монітор", 6500m), 1);
        cart.AddItem(new Product("Кабель HDMI", 150m), 3);

        cart.PrintItems();

        decimal total = cart.GetTotal();
        Console.WriteLine($"Підсумок кошика: {total:F2} грн");
        return total;
    }
}
