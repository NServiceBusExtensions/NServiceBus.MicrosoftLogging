<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> NServiceBus.MicrosoftLogging

[![Build status](https://ci.appveyor.com/api/projects/status/sovlo1pvgfh0xnba/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/nservicebus-MicrosoftLogging)
[![NuGet Status](https://img.shields.io/nuget/v/NServiceBus.MicrosoftLogging.svg)](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging/)

Add support for [NServiceBus](https://particular.net/nservicebus) to log to [Microsoft.Extensions.Logging](https://github.com/aspnet/Logging).

<!--- StartOpenCollectiveBackers -->

[Already a Patron? skip past this section](#endofbacking)


## Community backed

**It is expected that all developers either [become a Patron](https://opencollective.com/nservicebusextensions/contribute/patron-6976) or have a [Tidelift Subscription](#support-via-tidelift) to use NServiceBusExtensions. [Go to licensing FAQ](https://github.com/NServiceBusExtensions/Home/#licensingpatron-faq)**


### Sponsors

Support this project by [becoming a Sponsor](https://opencollective.com/nservicebusextensions/contribute/sponsor-6972). The company avatar will show up here with a website link. The avatar will also be added to all GitHub repositories under the [NServiceBusExtensions organization](https://github.com/NServiceBusExtensions).


### Patrons

Thanks to all the backing developers. Support this project by [becoming a patron](https://opencollective.com/nservicebusextensions/contribute/patron-6976).

<img src="https://opencollective.com/nservicebusextensions/tiers/patron.svg?width=890&avatarHeight=60&button=false">

<a href="#" id="endofbacking"></a>

<!--- EndOpenCollectiveBackers -->


## Support via TideLift

Support is available via a [Tidelift Subscription](https://tidelift.com/subscription/pkg/nuget-nservicebus.microsoftlogging?utm_source=nuget-nservicebus.microsoftlogging&utm_medium=referral&utm_campaign=enterprise).


<!-- toc -->
## Contents

  * [Usage](#usage)
  * [Usage when hosting](#usage-when-hosting)
    * [In a generic host](#in-a-generic-host)
    * [In a windows service](#in-a-windows-service)
  * [Security contact information](#security-contact-information)<!-- endToc -->


## NuGet package

https://nuget.org/packages/NServiceBus.MicrosoftLogging/
https://nuget.org/packages/NServiceBus.MicrosoftLogging.Hosting


## Usage

<!-- snippet: MsLoggingInCode -->
<a id='snippet-msloggingincode'></a>
```cs
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFilter(level => level >= MsLogLevel.Information);
    loggingBuilder.AddConsole();
});

using var loggerFactory = new LoggerFactory();
var logFactory = LogManager.Use<MicrosoftLogFactory>();
logFactory.UseMsFactory(loggerFactory);
// endpoint startup and shutdown
```
<sup><a href='/src/Tests/Snippets/Usage.cs#L11-L24' title='File snippet `msloggingincode` was extracted from'>snippet source</a> | <a href='#snippet-msloggingincode' title='Navigate to start of snippet `msloggingincode`'>anchor</a></sup>
<!-- endSnippet -->


## Usage when hosting

As `LoggerFactory` implements [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) it must be disposed of after stopping the NServiceBus endpoint. The process for doing this will depend on how the endpoint is being hosted.


### In a generic host

Disposing the `LoggerFactory` is done by the underlying infrastructure.

<!-- snippet: MsLoggingInGenericHost -->
<a id='snippet-msloggingingenerichost'></a>
```cs
var builder = Host.CreateDefaultBuilder();
builder.ConfigureLogging(logging => { logging.AddConsole(); });
// should go before any other Use or Configure method that uses NServiceBus
builder.UseMicrosoftLogFactoryLogging();
```
<sup><a href='/src/Tests/Snippets/GenericHostUsage.cs#L8-L15' title='File snippet `msloggingingenerichost` was extracted from'>snippet source</a> | <a href='#snippet-msloggingingenerichost' title='Navigate to start of snippet `msloggingingenerichost`'>anchor</a></sup>
<!-- endSnippet -->

Note: `UseMicrosoftLogFactoryLogger` requires adding `NServiceBus.MicrosoftLogging.Hosting` as a package dependency.

### In a windows service

When [hosting in a windows service](https://docs.particular.net/nservicebus/hosting/windows-service) `LoggerFactory` should be disposed of as part of the [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx) execution.

<!-- snippet: MsLoggingInService -->
<a id='snippet-mslogginginservice'></a>
```cs
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
```
<sup><a href='/src/Tests/Snippets/ProgramService.cs#L10-L74' title='File snippet `mslogginginservice` was extracted from'>snippet source</a> | <a href='#snippet-mslogginginservice' title='Navigate to start of snippet `mslogginginservice`'>anchor</a></sup>
<!-- endSnippet -->


## Security contact information

To report a security vulnerability, use the [Tidelift security contact](https://tidelift.com/security). Tidelift will coordinate the fix and disclosure.


## Icon

[Abstract](https://thenounproject.com/term/abstract/847344/) designed by [Neha Shinde](https://thenounproject.com/neha.shinde) from [The Noun Project](https://thenounproject.com).
