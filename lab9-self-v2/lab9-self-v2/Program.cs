using Lab9SelfV2;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DemoProducts();
DemoNumbers();
DemoStrings();
DemoLaziness();
DemoEmptyCollection();
DemoComparisonWithLinq();

// ---------- Колекція 1: List<Product> ----------
static void DemoProducts()
{
    Console.WriteLine("=== Колекція 1: List<Product> ===");

    var products = new List<Product>
    {
        new Product { Name = "Laptop", Price = 25000 },
        new Product { Name = "Mouse", Price = 500 },
        new Product { Name = "Keyboard", Price = 1500 },
        new Product { Name = "Monitor", Price = 8000 }
    };

    // 1. Map: список імен
    var names = CollectionUtils.Map(products, p => p.Name);
    Console.WriteLine("Names: " + string.Join(", ", names));

    // Map: ціни зі знижкою 10 %
    var discounted = CollectionUtils.Map(products, p => p.Price * 0.9m);
    Console.WriteLine("Ціни зі знижкою 10%: " + string.Join(", ", discounted));

    // 2. MaxBy: найдорожчий продукт і продукт з найдовшою назвою
    var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
    Console.WriteLine($"Most expensive: {mostExpensive.Name} - {mostExpensive.Price}");

    var longestName = CollectionUtils.MaxBy(products, p => p.Name.Length);
    Console.WriteLine($"Найдовша назва: {longestName.Name}");

    // 3. Filter: продукти дорожчі за 1000
    var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
    Console.WriteLine("Expensive products:");
    CollectionUtils.ForEach(expensive, p => Console.WriteLine($" - {p.Name}"));

    // 4. ForEach: усі продукти
    CollectionUtils.ForEach(products, p => Console.WriteLine($"{p.Name}: {p.Price} грн"));
    Console.WriteLine();
}

// ---------- Колекція 2: List<int> ----------
static void DemoNumbers()
{
    Console.WriteLine("=== Колекція 2: List<int> ===");

    var numbers = new List<int> { 3, 8, -2, 15, 7, 10, -9, 4 };

    var squares = CollectionUtils.Map(numbers, n => n * n);
    Console.WriteLine("Квадрати: " + string.Join(", ", squares));

    var evens = CollectionUtils.Filter(numbers, n => n % 2 == 0);
    Console.WriteLine("Парні: " + string.Join(", ", evens));

    var positive = CollectionUtils.Filter(numbers, n => n > 0);
    Console.WriteLine("Додатні: " + string.Join(", ", positive));

    int max = CollectionUtils.MaxBy(numbers, n => n);
    Console.WriteLine($"Максимум: {max}");

    int farthestFromZero = CollectionUtils.MaxBy(numbers, n => Math.Abs(n));
    Console.WriteLine($"Найдалі від нуля: {farthestFromZero}");

    // ForEach із накопиченням суми: лямбда захоплює локальну змінну (closure)
    int sum = 0;
    CollectionUtils.ForEach(numbers, n => sum += n);
    Console.WriteLine($"Сума через ForEach: {sum}");
    Console.WriteLine();
}

// ---------- Колекція 3: List<string> ----------
static void DemoStrings()
{
    Console.WriteLine("=== Колекція 3: List<string> ===");

    var words = new List<string> { "delegate", "lambda", "Func", "Action", "Predicate", "LINQ" };

    var upper = CollectionUtils.Map(words, w => w.ToUpper());
    Console.WriteLine("Верхній регістр: " + string.Join(", ", upper));

    var lengths = CollectionUtils.Map(words, w => $"{w}({w.Length})");
    Console.WriteLine("Довжини: " + string.Join(", ", lengths));

    var withA = CollectionUtils.Filter(words, w => w.Contains('a', StringComparison.OrdinalIgnoreCase));
    Console.WriteLine("Містять літеру 'a': " + string.Join(", ", withA));

    string longest = CollectionUtils.MaxBy(words, w => w.Length);
    Console.WriteLine($"Найдовше слово: {longest}");

    string alphabeticallyLast = CollectionUtils.MaxBy(words, w => w.ToLower());
    Console.WriteLine($"Останнє за алфавітом: {alphabeticallyLast}");

    CollectionUtils.ForEach(words, w => Console.Write($"[{w}] "));
    Console.WriteLine();
    Console.WriteLine();
}

// ---------- Відкладене виконання (як у LINQ) ----------
static void DemoLaziness()
{
    Console.WriteLine("=== Відкладене виконання Map і Filter ===");

    var numbers = new List<int> { 1, 2, 3 };
    var doubled = CollectionUtils.Map(numbers, n =>
    {
        Console.WriteLine($"  mapper викликано для {n}");
        return n * 2;
    });

    Console.WriteLine("Map викликано, але mapper ще не виконувався.");
    Console.WriteLine("Починаємо перебір:");
    CollectionUtils.ForEach(doubled, n => Console.WriteLine($"  отримано {n}"));
    Console.WriteLine();
}

// ---------- Порожня колекція та null ----------
static void DemoEmptyCollection()
{
    Console.WriteLine("=== Граничні випадки ===");

    try
    {
        CollectionUtils.MaxBy(new List<int>(), n => n);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"MaxBy для порожньої колекції: {ex.GetType().Name}: {ex.Message}");
    }

    try
    {
        CollectionUtils.Filter<int>(new List<int> { 1 }, null!);
    }
    catch (ArgumentNullException ex)
    {
        Console.WriteLine($"Filter з null-предикатом: {ex.GetType().Name} (параметр {ex.ParamName})");
    }

    Console.WriteLine();
}

// ---------- Порівняння з LINQ ----------
static void DemoComparisonWithLinq()
{
    Console.WriteLine("=== Порівняння з LINQ ===");

    var products = new List<Product>
    {
        new Product { Name = "Laptop", Price = 25000 },
        new Product { Name = "Mouse", Price = 500 },
        new Product { Name = "Keyboard", Price = 1500 }
    };

    bool mapSame = CollectionUtils.Map(products, p => p.Name)
        .SequenceEqual(products.Select(p => p.Name));
    bool filterSame = CollectionUtils.Filter(products, p => p.Price > 1000)
        .SequenceEqual(products.Where(p => p.Price > 1000));
    bool maxBySame = ReferenceEquals(
        CollectionUtils.MaxBy(products, p => p.Price),
        products.MaxBy(p => p.Price));

    Console.WriteLine($"Map    == Select : {mapSame}");
    Console.WriteLine($"Filter == Where  : {filterSame}");
    Console.WriteLine($"MaxBy  == MaxBy  : {maxBySame}");
}
