using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

public class IntegrationTests
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
        await Task.Delay(500);
        Assert.NotEmpty(LogMessageCapture.LoggingEvents);
        await endpoint.Stop();
    }

    [Fact]
    public Task Ensure_log_messages_are_redirected_in_Hosting_deferred()
    {
        return RunWithHost(true);
    }

    [Fact]
    public Task Ensure_log_messages_are_redirected_in_Hosting_not_deferred()
    {
        return RunWithHost(false);
    }

    static async Task RunWithHost(bool deferLogging)
    {
        var builder = Host.CreateDefaultBuilder();
        var logMessageCapture = new LogMessageCapture();
        builder.ConfigureLogging(logging => { logging.AddProvider(logMessageCapture); });
        builder.UseMicrosoftLogFactoryLogging(deferLogging);
        builder.UseNServiceBus(_ =>
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
        await Task.Delay(500);

        Assert.NotEmpty(LogMessageCapture.LoggingEvents);
        await host.StopAsync();
    }

    public IntegrationTests()
    {
        LogMessageCapture.LoggingEvents.Clear();
    }
}