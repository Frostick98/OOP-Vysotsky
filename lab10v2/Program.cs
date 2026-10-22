using System.Text;
using Lab10v2;

Console.OutputEncoding = Encoding.UTF8;

int[] source = { 42, 7, 19, 3, 88, 25, 1, 64 };

// 1. Колекція об'єктів типу інтерфейсу
Console.WriteLine("=== Інтерфейс ISortable ===");
List<ISortable> sorters = new()
{
    new BubbleSort(),
    new QuickSort()
};

Console.WriteLine($"Вхідний масив: {string.Join(", ", source)}");

foreach (ISortable sorter in sorters)
{
    int[] copy = (int[])source.Clone();
    sorter.Sort(copy);
    Console.WriteLine($"{sorter.GetType().Name,-11}-> {string.Join(", ", copy)}");
}

// 2. Колекція об'єктів типу абстрактного класу
Console.WriteLine();
Console.WriteLine("=== Абстрактний клас LoggerBase ===");
List<LoggerBase> loggers = new()
{
    new ConsoleLogger(),
    new DebugLogger()
};

foreach (LoggerBase logger in loggers)
{
    logger.LogTimestamp($"Повідомлення від {logger.GetType().Name}");
}

// 3. Спільна робота обох підходів: логер фіксує результат кожного сортування
Console.WriteLine();
Console.WriteLine("=== Спільна демонстрація ===");
LoggerBase common = loggers[0];

foreach (ISortable sorter in sorters)
{
    int[] copy = (int[])source.Clone();
    sorter.Sort(copy);
    common.LogTimestamp($"{sorter.GetType().Name} завершив: {string.Join(", ", copy)}");
}
