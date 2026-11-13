using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab14v2
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("Лабораторна робота №8. Варіант 2: CollectionHelper<T>.Map");

            // ---------- 1. List<int> ----------
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            Console.WriteLine("\n--- List<int> ---");
            Print("Вхідні числа", numbers);

            // int -> int
            IEnumerable<int> squares = CollectionHelper<int>.Map(numbers, n => n * n);
            Print("Квадрати (int -> int)", squares);

            // int -> string
            IEnumerable<string> numberLabels = CollectionHelper<int>.Map(numbers, n => $"Число #{n}");
            Print("Рядки (int -> string)", numberLabels);

            // ---------- 2. List<string> ----------
            var words = new List<string> { "яблуко", "груша", "банан", "ківі" };

            Console.WriteLine("\n--- List<string> ---");
            Print("Вхідні рядки", words);

            // string -> int
            IEnumerable<int> lengths = CollectionHelper<string>.Map(words, w => w.Length);
            Print("Довжини (string -> int)", lengths);

            // string -> string
            IEnumerable<string> upper = CollectionHelper<string>.Map(words, w => w.ToUpper());
            Print("Верхній регістр (string -> string)", upper);

            // ---------- 3. List<Product> ----------
            var products = new List<Product>
            {
                new Product(1, "Ноутбук", "Електроніка", 32000m),
                new Product(2, "Навушники", "Електроніка", 1500.50m),
                new Product(3, "Книга", "Книги", 250m),
                new Product(4, "Кава", "Продукти", 180m)
            };

            Console.WriteLine("\n--- List<Product> ---");
            Print("Вхідні продукти", products);

            // Product -> decimal (ціни), як у завданні
            IEnumerable<decimal> prices = CollectionHelper<Product>.Map(products, p => p.Price);
            Print("Ціни (Product -> decimal)", prices);

            // Product -> string
            IEnumerable<string> names = CollectionHelper<Product>.Map(products, p => p.Name);
            Print("Назви (Product -> string)", names);

            // ---------- 4. Граничні випадки ----------
            Console.WriteLine("\n--- Граничні випадки ---");

            var empty = new List<int>();
            Print("Порожня колекція", CollectionHelper<int>.Map(empty, n => n * 2));

            try
            {
                CollectionHelper<int>.Map(numbers, (Func<int, int>)null!);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Помилка: параметр '{ex.ParamName}' не може бути null");
            }
        }

        // Допоміжний метод для виводу будь-якої колекції в консоль.
        private static void Print<TItem>(string title, IEnumerable<TItem> items)
        {
            Console.WriteLine($"{title}: [{string.Join(", ", items)}]");
        }
    }
}
