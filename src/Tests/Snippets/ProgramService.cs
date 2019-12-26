﻿using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

#region MsLoggingInService

using MsLogLevel = Microsoft.Extensions.Logging.LogLevel;
using MsLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

[DesignerCategory("Code")]
class ProgramService :
    ServiceBase
{
    IEndpointInstance? endpointInstance;
    Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory;

    static void Main()
    {
        using var service = new ProgramService();
        if (ServiceHelper.IsService())
        {
            Run(service);
            return;
        }
        service.OnStart(null);
        Console.WriteLine("Bus started. Press any key to exit");
        Console.ReadKey();
        service.OnStop();
    }

    protected override void OnStart(string[]? args)
    {
        AsyncOnStart().GetAwaiter().GetResult();
    }

    async Task AsyncOnStart()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder =>
        {
            builder.AddFilter(level => level >= MsLogLevel.Information);
            builder.AddConsole();
        });

        var serviceProvider = services.BuildServiceProvider();

        loggerFactory = serviceProvider.GetService<MsLoggerFactory>();
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(loggerFactory);
        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        endpointConfiguration.EnableInstallers();
        endpointInstance = await Endpoint.Start(endpointConfiguration);
    }

    protected override void OnStop()
    {
        AsyncOnStop().GetAwaiter().GetResult();
    }

    async Task AsyncOnStop()
    {
        if (endpointInstance != null)
        {
            await endpointInstance.Stop();
        }
        loggerFactory?.Dispose();
    }
}
#endregion

class ServiceHelper
{
    public static bool IsService()
    {
        throw new NotImplementedException();
    }
}