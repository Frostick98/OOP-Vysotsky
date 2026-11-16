using System.Diagnostics;
using System.Text;
using Lab15v2;

Console.OutputEncoding = Encoding.UTF8;

var processor = new LogProcessor();

// ================= 1. HashSet<string> =================
PrintHeader("1. HashSet<T> - унікальні IP-адреси");

string[] ips =
{
    "192.168.0.10", "10.0.0.5", "192.168.0.10",
    "172.16.1.7", "10.0.0.5", "192.168.0.10"
};

foreach (string ip in ips)
{
    bool added = processor.RegisterIp(ip);
    Console.WriteLine($"Add(\"{ip}\") -> {(added ? "додано" : "дублікат, пропущено")}");
}

Console.WriteLine($"Усього надійшло: {ips.Length}, унікальних: {processor.UniqueIpCount}");
Console.WriteLine($"Contains(\"10.0.0.5\")  -> {processor.ContainsIp("10.0.0.5")}");
Console.WriteLine($"Contains(\"8.8.8.8\")   -> {processor.ContainsIp("8.8.8.8")}");
Console.WriteLine($"Remove(\"172.16.1.7\")  -> {processor.RemoveIp("172.16.1.7")}");
Console.WriteLine($"Унікальні IP зараз: {string.Join(", ", processor.UniqueIps)}");

// Операції над множинами
var yesterday = new HashSet<string> { "10.0.0.5", "192.168.0.10", "203.0.113.9" };
var today = new HashSet<string> { "10.0.0.5", "198.51.100.4", "203.0.113.9", "192.168.0.77" };

var union = new HashSet<string>(yesterday);
union.UnionWith(today);

var common = new HashSet<string>(yesterday);
common.IntersectWith(today);

var onlyToday = new HashSet<string>(today);
onlyToday.ExceptWith(yesterday);

Console.WriteLine($"UnionWith     (усі IP за два дні): {string.Join(", ", union)}");
Console.WriteLine($"IntersectWith (були в обидва дні): {string.Join(", ", common)}");
Console.WriteLine($"ExceptWith    (нові сьогодні):     {string.Join(", ", onlyToday)}");

// Порівняння продуктивності Contains: List<T> та HashSet<T>
const int N = 20_000;
var list = new List<string>(N);
var set = new HashSet<string>();
for (int i = 0; i < N; i++)
{
    string ip = $"10.{i / 65536 % 256}.{i / 256 % 256}.{i % 256}";
    list.Add(ip);
    set.Add(ip);
}

const string target = "not-found"; // найгірший випадок: елемента немає
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++) { list.Contains(target); }
sw.Stop();
long listMs = sw.ElapsedMilliseconds;

sw.Restart();
for (int i = 0; i < 1000; i++) { set.Contains(target); }
sw.Stop();
long setMs = sw.ElapsedMilliseconds;

Console.WriteLine($"1000 пошуків у колекції з {N} IP: List<T> = {listMs} мс (O(n)), " +
                  $"HashSet<T> = {setMs} мс (O(1) в середньому)");

// ================= 2. Queue<LogEntry> =================
PrintHeader("2. Queue<T> - черга логів для обробки (FIFO)");

var baseTime = new DateTime(2026, 11, 16, 9, 0, 0);
var logs = new[]
{
    new LogEntry(1, baseTime.AddSeconds(0),  "192.168.0.10", "INFO",  "Користувач увійшов у систему"),
    new LogEntry(2, baseTime.AddSeconds(5),  "10.0.0.5",     "WARN",  "Повільна відповідь сервера"),
    new LogEntry(3, baseTime.AddSeconds(9),  "203.0.113.9",  "ERROR", "Помилка підключення до БД"),
    new LogEntry(4, baseTime.AddSeconds(14), "192.168.0.10", "INFO",  "Завантажено файл звіту"),
    new LogEntry(5, baseTime.AddSeconds(20), "198.51.100.4", "ERROR", "Невдала спроба авторизації"),
};

var queueProcessor = new LogProcessor();
foreach (LogEntry log in logs)
{
    queueProcessor.EnqueueLog(log);
    Console.WriteLine($"Enqueue -> {log}");
}

Console.WriteLine($"У черзі: {queueProcessor.QueueCount}, унікальних IP: {queueProcessor.UniqueIpCount}");
Console.WriteLine($"Peek (перший у черзі, не видаляється): {queueProcessor.PeekNextLog()}");
Console.WriteLine($"Після Peek у черзі досі: {queueProcessor.QueueCount}");

Console.WriteLine("Обробка двох перших логів (Dequeue) - порядок збігається з порядком надходження:");
for (int i = 0; i < 2; i++)
{
    Console.WriteLine($"Dequeue -> {queueProcessor.ProcessNextLog()}");
}

Console.WriteLine($"Залишилось у черзі: {queueProcessor.QueueCount}");

// ================= 3. Stack<LogEntry> =================
PrintHeader("3. Stack<T> - історія переглянутих логів (LIFO)");

// Решту логів теж «переглядаємо» - вони потрапляють в історію
while (queueProcessor.QueueCount > 0)
{
    LogEntry? viewed = queueProcessor.ProcessNextLog();
    Console.WriteLine($"Переглянуто й додано в історію (Push) -> {viewed}");
}

Console.WriteLine($"Розмір історії: {queueProcessor.HistoryCount}");
Console.WriteLine($"Peek (вершина історії, не видаляється): {queueProcessor.PeekLastViewed()}");

Console.WriteLine("Історія від вершини до дна (останній переглянутий - перший):");
foreach (LogEntry log in queueProcessor.GetHistory())
{
    Console.WriteLine($"  {log}");
}

Console.WriteLine("Кроки «назад» (Pop) - логи повертаються в зворотному порядку:");
while (queueProcessor.HistoryCount > 0)
{
    Console.WriteLine($"Pop -> {queueProcessor.GoBack()}");
}

Console.WriteLine($"Історія порожня: {queueProcessor.HistoryCount == 0}");
Console.WriteLine($"Peek на порожній історії -> {(queueProcessor.PeekLastViewed() is null ? "null" : "є лог")}");

Console.WriteLine();
Console.WriteLine("Готово. Натисніть Enter для завершення...");
Console.ReadLine();

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 70));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 70));
}
