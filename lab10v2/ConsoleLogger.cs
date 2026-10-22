namespace Lab10v2;

/// <summary>
/// Логер, що виводить повідомлення у консоль.
/// </summary>
public class ConsoleLogger : LoggerBase
{
    public override void WriteLog(string message)
    {
        Console.WriteLine($"[Console] {message}");
    }
}
