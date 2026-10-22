using System.Diagnostics;

namespace Lab10v2;

/// <summary>
/// Логер, що пише у вікно налагодження (Debug) і дублює запис у консоль для демонстрації.
/// </summary>
public class DebugLogger : LoggerBase
{
    public override void WriteLog(string message)
    {
        Debug.WriteLine(message);
        Console.WriteLine($"[Debug]   {message}");
    }
}
