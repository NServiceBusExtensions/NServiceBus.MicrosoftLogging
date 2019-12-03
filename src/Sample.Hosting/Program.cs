using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

public static class Program
{
    public static async Task Main()
    {
        using var host = CreateHostBuilder().Build();
        await host.StartAsync();

        var messageSession = host.Services.GetService<IMessageSession>();

        var message = new MyMessage
        {
            DateSend = DateTime.Now,
        };
        await messageSession.SendLocal(message);
        Console.WriteLine("\r\nPress any key to stop program\r\n");

        Console.Read();
        await host.StopAsync();
    }

    static IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
        // should go before UseNServiceBus
        builder.UseMicrosoftLogFactoryLogging();
        builder.UseNServiceBus(ctx =>
        {
            var configuration = new EndpointConfiguration("MicrosoftLoggingSample");
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseTransport<LearningTransport>();
            configuration.SendFailedMessagesTo("error");
            return configuration;
        });
        return builder;
    }
}