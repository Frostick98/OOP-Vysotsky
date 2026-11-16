namespace Lab15v2;

/// <summary>
/// Обробка логів:
///  - HashSet&lt;string&gt;   : унікальні IP-адреси;
///  - Queue&lt;LogEntry&gt;   : черга логів для обробки (FIFO);
///  - Stack&lt;LogEntry&gt;   : історія переглянутих логів (LIFO).
/// </summary>
public class LogProcessor
{
    private readonly HashSet<string> _uniqueIps = new();
    private readonly Queue<LogEntry> _queue = new();
    private readonly Stack<LogEntry> _viewedHistory = new();

    public int UniqueIpCount => _uniqueIps.Count;
    public int QueueCount => _queue.Count;
    public int HistoryCount => _viewedHistory.Count;
    public IReadOnlyCollection<string> UniqueIps => _uniqueIps;

    // ---------- HashSet: унікальні IP-адреси ----------

    /// <summary>Додає IP; повертає false, якщо така адреса вже є.</summary>
    public bool RegisterIp(string ip) => _uniqueIps.Add(ip);

    public bool ContainsIp(string ip) => _uniqueIps.Contains(ip);

    public bool RemoveIp(string ip) => _uniqueIps.Remove(ip);

    // ---------- Queue: черга логів на обробку ----------

    /// <summary>Ставить лог у кінець черги та реєструє його IP.</summary>
    public void EnqueueLog(LogEntry log)
    {
        _queue.Enqueue(log);
        RegisterIp(log.IpAddress);
    }

    /// <summary>Переглядає перший лог у черзі, не видаляючи його.</summary>
    public LogEntry? PeekNextLog() => _queue.Count > 0 ? _queue.Peek() : null;

    /// <summary>Бере лог з початку черги й додає його в історію переглянутих.</summary>
    public LogEntry? ProcessNextLog()
    {
        if (_queue.Count == 0)
        {
            return null;
        }

        LogEntry log = _queue.Dequeue();
        _viewedHistory.Push(log);
        return log;
    }

    // ---------- Stack: історія переглянутих логів ----------

    /// <summary>Переглядає останній переглянутий лог, не видаляючи його.</summary>
    public LogEntry? PeekLastViewed() => _viewedHistory.Count > 0 ? _viewedHistory.Peek() : null;

    /// <summary>Крок «назад» в історії: повертає останній переглянутий лог.</summary>
    public LogEntry? GoBack() => _viewedHistory.Count > 0 ? _viewedHistory.Pop() : null;

    public IEnumerable<LogEntry> GetHistory() => _viewedHistory; // від вершини до дна
}
