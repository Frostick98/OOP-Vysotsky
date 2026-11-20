// ============================================================================
// Лабораторна робота №12. Обробка IO/мережевих помилок та патерн Retry.
// Дисципліна: Об'єктно-орієнтоване програмування
// Студент: Висоцький Олександр, група ІПЗ-3/1
// Варіант 2. Сценарій: Запис у файл.
//
//   FileProcessor.WriteToFile(path, content)  - IOException перші 3 рази
//   NetworkClient.UploadFile(url, filePath)   - HttpRequestException перші 2 рази
//   shouldRetry: повторювати для IOException та HttpRequestException
// ============================================================================

using System.Diagnostics;
using System.Text;

namespace Lab18v2;

// ----------------------------------------------------------------------------
// Імітація роботи з файлами
// ----------------------------------------------------------------------------
public class FileProcessor
{
    private int _calls;

    /// <summary>Імітує запис у файл: перші 3 виклики кидають IOException.</summary>
    public void WriteToFile(string path, string content)
    {
        _calls++;
        Console.WriteLine($"      FileProcessor.WriteToFile: виклик №{_calls}, файл \"{path}\"");
        try
        {
            if (_calls <= 3)
                throw new IOException($"Файл \"{path}\" тимчасово зайнятий іншим процесом");

            Console.WriteLine($"      Записано {content.Length} символів у \"{path}\"");
        }
        finally
        {
            // finally виконується і при помилці, і при успіху: імітація закриття потоку
            Console.WriteLine("      (finally) Потік файлу закрито");
        }
    }
}

// ----------------------------------------------------------------------------
// Імітація мережевих запитів
// ----------------------------------------------------------------------------
public class NetworkClient
{
    private int _calls;

    /// <summary>Імітує завантаження файлу на сервер: перші 2 виклики кидають HttpRequestException.</summary>
    public bool UploadFile(string url, string filePath)
    {
        _calls++;
        Console.WriteLine($"      NetworkClient.UploadFile: виклик №{_calls}, {url}");
        if (_calls <= 2)
            throw new HttpRequestException("Сервер тимчасово недоступний (503)");

        Console.WriteLine($"      Файл \"{filePath}\" успішно завантажено");
        return true;
    }
}

// ----------------------------------------------------------------------------
// Узагальнений допоміжний клас Retry з експоненційною затримкою
// ----------------------------------------------------------------------------
public static class RetryHelper
{
    private static readonly TimeSpan FallbackDelay = TimeSpan.FromMilliseconds(500);

    /// <param name="operation">Операція, яку потрібно виконати.</param>
    /// <param name="retryCount">Максимальна кількість ПОВТОРНИХ спроб (усього спроб = retryCount + 1).</param>
    /// <param name="initialDelay">Початкова затримка; затримка перед повтором №k = initialDelay * 2^(k-1).</param>
    /// <param name="shouldRetry">Визначає, чи повторювати спробу для конкретного винятку (null = для всіх).</param>
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (initialDelay == default) initialDelay = FallbackDelay;
        int totalAttempts = retryCount + 1;

        for (int attempt = 0; ; attempt++)
        {
            Console.WriteLine($"  Спроба {attempt + 1} з {totalAttempts}...");
            try
            {
                T result = operation();
                Console.WriteLine($"  Спроба {attempt + 1}: УСПІХ");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Спроба {attempt + 1}: НЕВДАЧА, {ex.GetType().Name}: {ex.Message}");

                // 1) виняток не підлягає повтору (вибіркова обробка)
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.WriteLine($"  Виняток {ex.GetType().Name} не підлягає повтору, зупиняємось");
                    throw;
                }

                // 2) повторні спроби вичерпано
                if (attempt >= retryCount)
                {
                    Console.WriteLine($"  Усі повторні спроби ({retryCount}) вичерпано");
                    throw;
                }

                // 3) експоненційна затримка: initialDelay * 2^attempt
                TimeSpan delay = TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * Math.Pow(2, attempt));
                Console.WriteLine($"  Повтор через {delay.TotalMilliseconds:F0} мс (= {initialDelay.TotalMilliseconds:F0} * 2^{attempt})");
                Thread.Sleep(delay);
            }
        }
    }

    // Зручне перевантаження для методів, що повертають void
    public static void ExecuteWithRetry(
        Action operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
        => ExecuteWithRetry<bool>(() => { operation(); return true; }, retryCount, initialDelay, shouldRetry);
}

public static class Program
{
    // shouldRetry варіанту: повторювати лише для IOException та HttpRequestException
    private static readonly Func<Exception, bool> RetryPolicy =
        ex => ex is IOException || ex is HttpRequestException;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var delay = TimeSpan.FromMilliseconds(200);

        // ---------------- Сценарій 1: FileProcessor (IOException x3, потім успіх) ----------------
        Console.WriteLine("=== Сценарій 1: запис у файл (3 невдачі, потім успіх) ===");
        var files = new FileProcessor();
        var sw = Stopwatch.StartNew();
        RetryHelper.ExecuteWithRetry(
            () => files.WriteToFile("report.txt", "Тестовий вміст файлу"),
            retryCount: 3, initialDelay: delay, shouldRetry: RetryPolicy);
        Console.WriteLine($"Сценарій 1 завершено за {sw.ElapsedMilliseconds} мс\n");

        // ---------------- Сценарій 2: NetworkClient (HttpRequestException x2, потім успіх) ----------------
        Console.WriteLine("=== Сценарій 2: завантаження файлу (2 невдачі, потім успіх) ===");
        var net = new NetworkClient();
        sw.Restart();
        bool uploaded = RetryHelper.ExecuteWithRetry(
            () => net.UploadFile("https://example.com/upload", "report.txt"),
            retryCount: 3, initialDelay: delay, shouldRetry: RetryPolicy);
        Console.WriteLine($"Сценарій 2 завершено за {sw.ElapsedMilliseconds} мс, результат: {uploaded}\n");

        // ---------------- Сценарій 3: shouldRetry (виняток не з переліку) ----------------
        Console.WriteLine("=== Сценарій 3: shouldRetry (UnauthorizedAccessException не повторюється) ===");
        try
        {
            RetryHelper.ExecuteWithRetry<bool>(
                () => throw new UnauthorizedAccessException("Немає прав на запис"),
                retryCount: 3, initialDelay: delay, shouldRetry: RetryPolicy);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Main перехопив: {ex.GetType().Name}\n");
        }

        // ---------------- Сценарій 4: повторні спроби вичерпано ----------------
        Console.WriteLine("=== Сценарій 4: недостатньо спроб (retryCount = 1, потрібно 2 повтори) ===");
        var net2 = new NetworkClient();
        try
        {
            RetryHelper.ExecuteWithRetry(
                () => net2.UploadFile("https://example.com/upload", "report.txt"),
                retryCount: 1, initialDelay: delay, shouldRetry: RetryPolicy);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Main перехопив: {ex.GetType().Name}: {ex.Message}");
        }
    }
}

// ============================================================================
// ВИСНОВОК:
// Затримка між спробами зростає експоненційно: перед повтором №k чекаємо
// initialDelay * 2^(k-1). У цьому варіанті при initialDelay = 200 мс паузи
// становлять 200, 400 та 800 мс, тому запис у файл (3 невдачі) завершується
// успіхом на 4-й спробі, а завантаження (2 невдачі) на 3-й. Зростання пауз
// дає тимчасовій помилці час зникнути і не перевантажує ресурс частими запитами.
// ============================================================================
