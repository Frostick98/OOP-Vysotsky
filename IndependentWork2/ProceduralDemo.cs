/*
 * ПРОБЛЕМИ ПРОЦЕДУРНОЇ ВЕРСІЇ
 *
 * 1. Дані про один товар розкидані по трьох паралельних масивах (names, prices,
 *    quantities). Щоб отримати "товар №2", треба щоразу звертатися до трьох
 *    масивів з одним індексом.
 * 2. Немає гарантії, що масиви однакової довжини: якщо додати назву, але
 *    забути ціну, програма впаде з IndexOutOfRangeException або порахує
 *    неправильно.
 * 3. Додавання нового поля (наприклад, категорії чи артикула) вимагає нового
 *    масиву і зміни сигнатур усіх методів, які передають дані далі.
 * 4. Логіка знижки (CalculateDiscount) приймає "голі" числа й не прив'язана
 *    до товару, тож її важко повторно використати для іншого сценарію
 *    (наприклад, знижка залежно від категорії).
 * 5. Немає єдиної сутності "кошик": щоб видалити товар, потрібно вручну
 *    видаляти елемент з кожного масиву й тримати їх синхронними.
 * 6. Параметри методів розростаються: щоб передати кошик в інше місце програми,
 *    потрібно передавати всі масиви одразу.
 */

public partial class Program
{
    private const decimal DiscountThreshold = 500m;
    private const decimal DiscountRate = 0.10m;

    // Сума по позиції: ціна * кількість
    private static decimal CalculateLineTotal(decimal price, int quantity)
    {
        return price * quantity;
    }

    // Знижка 10% для товарів, ціна яких більша за 500 грн
    private static decimal CalculateDiscount(decimal price, decimal lineTotal)
    {
        return price > DiscountThreshold ? lineTotal * DiscountRate : 0m;
    }

    // Підсумок кошика
    private static decimal CalculateCartTotal(decimal[] finalLineTotals)
    {
        decimal total = 0m;
        for (int i = 0; i < finalLineTotals.Length; i++)
        {
            total += finalLineTotals[i];
        }
        return total;
    }

    public static decimal RunProceduralDemo()
    {
        // Паралельні масиви: дані про один товар розкидані по трьох масивах
        string[] names = { "Мишка", "Клавіатура", "Монітор", "Кабель HDMI" };
        decimal[] prices = { 350m, 800m, 6500m, 150m };
        int[] quantities = { 2, 1, 1, 3 };

        decimal[] finalLineTotals = new decimal[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            decimal lineTotal = CalculateLineTotal(prices[i], quantities[i]);
            decimal discount = CalculateDiscount(prices[i], lineTotal);
            finalLineTotals[i] = lineTotal - discount;

            Console.Write($"{names[i]}: {prices[i]:F2} x {quantities[i]} = {lineTotal:F2} грн");
            if (discount > 0)
            {
                Console.Write($", знижка {discount:F2} грн, до сплати {finalLineTotals[i]:F2} грн");
            }
            Console.WriteLine();
        }

        decimal total = CalculateCartTotal(finalLineTotals);
        Console.WriteLine($"Підсумок кошика: {total:F2} грн");
        return total;
    }
}
