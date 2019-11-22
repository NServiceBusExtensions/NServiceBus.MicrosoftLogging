using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class GenericHostUsage
{
    GenericHostUsage()
    {
        #region MsLoggingInGenericHost

        Host.CreateDefaultBuilder()
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            })
            // should go before any other Use or Configure method that uses NServiceBus
            .UseMicrosoftLogFactoryLogging();
        #endregion
    }
}