using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public static class Program
{
    static Program()
    {
        //HACK: Force US culture to work around https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
    }

    public static async Task Main()
    {
        using (var loggerFactory = new LoggerFactory())
        {
            loggerFactory.AddConsole();
            var logFactory = LogManager.Use<MicrosoftLogFactory>();
            logFactory.UseMsFactory(loggerFactory);

            var endpointConfiguration = new EndpointConfiguration("MicrosoftLoggingSample");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.SendFailedMessagesTo("error");
            var endpoint = await Endpoint.Start(endpointConfiguration);
            try
            {
                var message = new MyMessage
                {
                    DateSend = DateTime.Now,
                };
                await endpoint.SendLocal(message);
                Console.WriteLine("\r\nPress any key to stop program\r\n");
                Console.Read();
            }
            finally
            {
                await endpoint.Stop();
            }
        }
    }

}