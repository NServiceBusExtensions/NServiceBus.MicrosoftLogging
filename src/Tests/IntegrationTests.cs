using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

public class IntegrationTests :
    XunitApprovalBase
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
        Assert.NotEmpty(logMessageCapture.LoggingEvents);
        await endpoint.Stop();
    }

    public IntegrationTests(ITestOutputHelper output) :
        base(output)
    {
    }
}