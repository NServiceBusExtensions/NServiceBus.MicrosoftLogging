using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

public class IntegrationTests :
    VerifyBase
{
    [Fact]
    public async Task Ensure_log_messages_are_redirected()
    {
        using var msLoggerFactory = new LoggerFactory();
        var logMessageCapture = new LogMessageCapture();
        msLoggerFactory.AddProvider(logMessageCapture);
        var logFactory = LogManager.Use<MicrosoftLogFactory>();
        logFactory.UseMsFactory(msLoggerFactory);

        var configuration = new EndpointConfiguration("Tests");
        configuration.UseTransport<LearningTransport>();

        var endpoint = await Endpoint.Start(configuration);

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await endpoint.SendLocal(message);
        Assert.NotEmpty(LogMessageCapture.LoggingEvents);
        await endpoint.Stop();
    }

    [Fact]
    public async Task Ensure_log_messages_are_redirected_in_Hosting_deferred()
    {
        await RunWithHost(true);
    }

    [Fact]
    public async Task Ensure_log_messages_are_redirected_in_Hosting_not_deferred()
    {
        await RunWithHost(false);
    }

    static async Task RunWithHost(bool deferLogging)
    {
        var builder = Host.CreateDefaultBuilder();
        var logMessageCapture = new LogMessageCapture();
        builder.ConfigureLogging(logging => { logging.AddProvider(logMessageCapture); });
        builder.UseMicrosoftLogFactoryLogging(deferLogging);
        builder.UseNServiceBus(ctx =>
        {
            var configuration = new EndpointConfiguration("HostingTest");
            configuration.UseTransport<LearningTransport>();
            return configuration;
        });

        using var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetService<IMessageSession>();

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await messageSession.SendLocal(message);

        Assert.NotEmpty(LogMessageCapture.LoggingEvents);
        await host.StopAsync();
    }

    public IntegrationTests(ITestOutputHelper output) :
        base(output)
    {
        LogMessageCapture.LoggingEvents.Clear();
    }
}