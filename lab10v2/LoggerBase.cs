namespace Lab10v2;

/// <summary>
/// Базовий клас логера. Конкретний метод <see cref="LogTimestamp"/> додає час
/// і делегує запис абстрактному методу <see cref="WriteLog"/>.
/// </summary>
public abstract class LoggerBase
{
    /// <summary>Записує готове повідомлення у конкретний приймач (консоль, Debug тощо).</summary>
    public abstract void WriteLog(string message);

    /// <summary>Додає до повідомлення поточний час і записує його через <see cref="WriteLog"/>.</summary>
    public void LogTimestamp(string message)
    {
        string stamped = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        WriteLog(stamped);
    }
}
