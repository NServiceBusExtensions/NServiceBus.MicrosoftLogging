using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class GenericHostUsage
{
    GenericHostUsage()
    {
        #region MsLoggingInGenericHost

        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureLogging(logging => { logging.AddConsole(); });
        // should go before any other Use or Configure method that uses NServiceBus
        builder.UseMicrosoftLogFactoryLogging();

        #endregion
    }
}