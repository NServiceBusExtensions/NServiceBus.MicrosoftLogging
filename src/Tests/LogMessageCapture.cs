using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

class LogMessageCapture : ILoggerProvider, ILogger
{
    public List<string> LoggingEvents = new List<string>();

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var text = formatter.Invoke(state, exception);
        LoggingEvents.Add(text);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return this;
    }
}