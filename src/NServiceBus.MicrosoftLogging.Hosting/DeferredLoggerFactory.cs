using NServiceBus.Logging;

class DeferredLoggerFactory :
    ILoggerFactory
{
    public ConcurrentDictionary<string, ConcurrentQueue<(LogLevel level, string message)>> deferredLogs = new();

    public DeferredLoggerFactory(LogLevel filterLevel)
    {
        this.filterLevel = filterLevel;
        isDebugEnabled = LogLevel.Debug >= filterLevel;
        isInfoEnabled = LogLevel.Info >= filterLevel;
        isWarnEnabled = LogLevel.Warn >= filterLevel;
        isErrorEnabled = LogLevel.Error >= filterLevel;
        isFatalEnabled = LogLevel.Fatal >= filterLevel;
    }

    public ILog GetLogger(Type type)
    {
        return GetLogger(type.FullName);
    }

    public ILog GetLogger(string name)
    {
        return new NamedLogger(name, this)
        {
            IsDebugEnabled = isDebugEnabled,
            IsInfoEnabled = isInfoEnabled,
            IsWarnEnabled = isWarnEnabled,
            IsErrorEnabled = isErrorEnabled,
            IsFatalEnabled = isFatalEnabled
        };
    }

    public void Write(string name, LogLevel messageLevel, string message)
    {
        if (messageLevel < filterLevel)
        {
            return;
        }
        var logQueues = deferredLogs.GetOrAdd(name, new ConcurrentQueue<(LogLevel level, string message)>());
        logQueues.Enqueue((messageLevel, message));
    }

    LogLevel filterLevel;
    bool isDebugEnabled;
    bool isErrorEnabled;
    bool isFatalEnabled;
    bool isInfoEnabled;
    bool isWarnEnabled;
}