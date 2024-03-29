using Microsoft.Extensions.Logging;

class LogMessageCapture :
    ILoggerProvider,
    ILogger
{
    public static ConcurrentBag<string> LoggingEvents = [];

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName) =>
        this;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var text = formatter.Invoke(state, exception);
        LoggingEvents.Add(text);
    }

    public bool IsEnabled(LogLevel logLevel) =>
        true;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull =>
        this;
}