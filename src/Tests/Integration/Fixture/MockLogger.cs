using Microsoft.Extensions.Logging;

namespace Integration.Fixture;

internal class MockLogger<TCategoryName> : ILogger<TCategoryName>
{
    private readonly List<LogEntry> _entries = new();

    private readonly LogLevel _minimumLevel;

    private MockLogger(LogLevel minimumLevel)
    {
        _minimumLevel = minimumLevel;
    }

    public string GetAllMessages() => GetAllMessages(null!);

    public MockLogger() : this(LogLevel.Trace) { }

    public IDisposable BeginScope<TState>(TState state) => new MockLoggerScope();

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _minimumLevel;

    public void Log<TState>
    (
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception, string> formatter
    )
    {
        if (IsEnabled(logLevel))
            _entries.Add(new LogEntry
            { Level = logLevel, Message = exception?.Message + formatter(state, exception!) });
    }

    private string GetAllMessages(string? separator) =>
        string.Join(
            separator ?? Environment.NewLine,
            _entries.Select(logEntry => logEntry.Message));
}

internal class LogEntry
{
    public LogLevel Level { get; set; }

    public string Message { get; set; } = default!;
}

internal class MockLoggerScope : IDisposable
{
    public void Dispose() { }
}
