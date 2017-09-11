using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    [Test]
    public async Task Ensure_log_messages_are_redirected()
    {
        using (var msLoggerFactory = new LoggerFactory())
        {
            var logMessageCapture = new LogMessageCapture();
            msLoggerFactory.AddProvider(logMessageCapture);
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(msLoggerFactory);

            var endpointConfiguration = new EndpointConfiguration("Tests");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var endpoint = await Endpoint.Start(endpointConfiguration);
            Assert.IsNotEmpty(logMessageCapture.LoggingEvents);
            await endpoint.Stop();
        }
    }
}