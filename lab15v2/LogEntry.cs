namespace Lab15v2;

/// <summary>Один запис журналу (лог).</summary>
public class LogEntry
{
    public int Id { get; }
    public DateTime Timestamp { get; }
    public string IpAddress { get; }
    public string Level { get; }
    public string Message { get; }

    public LogEntry(int id, DateTime timestamp, string ipAddress, string level, string message)
    {
        Id = id;
        Timestamp = timestamp;
        IpAddress = ipAddress;
        Level = level;
        Message = message;
    }

    public override string ToString() =>
        $"#{Id} [{Timestamp:HH:mm:ss}] {Level,-5} {IpAddress,-15} {Message}";
}
