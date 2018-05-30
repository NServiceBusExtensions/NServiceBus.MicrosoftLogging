using System;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using NServiceBus.Logging;
using MsLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

class LoggerFactory : ILoggerFactory
{
    MsLoggerFactory msFactory;

    public LoggerFactory(MsLoggerFactory msFactory)
    {
        this.msFactory = msFactory;
    }

    public ILog GetLogger(Type type)
    {
        return GetLogger(TypeNameHelper.GetTypeDisplayName(type));
    }

    public ILog GetLogger(string name)
    {
        var logger = msFactory.CreateLogger(name);
        return new Logger(logger);
    }
}