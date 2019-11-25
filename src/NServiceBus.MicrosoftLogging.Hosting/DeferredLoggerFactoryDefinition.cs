using System;
using NServiceBus.Logging;

class DeferredLoggerFactoryDefinition : LoggingFactoryDefinition
{
    public DeferredLoggerFactoryDefinition()
    {
        level = new Lazy<LogLevel>(() => LogLevel.Info);
    }

    protected override ILoggerFactory GetLoggingFactory()
    {
        Factory = new DeferredLoggerFactory(level.Value);
        return Factory;
    }

    public static DeferredLoggerFactory? Factory;

    Lazy<LogLevel> level;
}