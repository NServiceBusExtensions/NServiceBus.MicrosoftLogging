using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public static class Program
{
    public static async Task Main()
    {
        using (var loggerFactory = new LoggerFactory())
        {
            loggerFactory.AddConsole();
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);

            var configuration = new EndpointConfiguration("MicrosoftLoggingSample");
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseTransport<LearningTransport>();
            configuration.SendFailedMessagesTo("error");
            var endpoint = await Endpoint.Start(configuration);
            var message = new MyMessage
            {
                DateSend = DateTime.Now,
            };
            await endpoint.SendLocal(message);
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.Read();
            await endpoint.Stop();
        }
    }
}