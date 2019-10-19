<!--
GENERATED FILE - DO NOT EDIT
This file was generated by [MarkdownSnippets](https://github.com/SimonCropp/MarkdownSnippets).
Source File: /readme.source.md
To change this file edit the source file and then run MarkdownSnippets.
-->

# <img src="/src/icon.png" height="30px"> NServiceBus.Hyperion

[![Build status](https://ci.appveyor.com/api/projects/status/20f8p78334a1utj4/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/nservicebus-Hyperion)
[![NuGet Status](https://img.shields.io/nuget/v/NServiceBus.Hyperion.svg?cacheSeconds=86400)](https://www.nuget.org/packages/NServiceBus.Hyperion/)

Add support for [NServiceBus](https://particular.net/nservicebus) to log to [Microsoft.Extensions.Logging](https://github.com/aspnet/Logging).

<!--- StartOpenCollectiveBackers -->

[Already a Patron? skip past this section](#endofbacking)


## Community backed

**It is expected that all developers [become a Patron](https://opencollective.com/nservicebusextensions/order/6976) to use any of these libraries. [Go to licensing FAQ](https://github.com/NServiceBusExtensions/Home/blob/master/readme.md#licensingpatron-faq)**


### Sponsors

Support this project by [becoming a Sponsors](https://opencollective.com/nservicebusextensions/order/6972). The company avatar will show up here with a link to your website. The avatar will also be added to all GitHub repositories under this organization.


### Patrons

Thanks to all the backing developers! Support this project by [becoming a patron](https://opencollective.com/nservicebusextensions/order/6976).

<img src="https://opencollective.com/nservicebusextensions/tiers/patron.svg?width=890&avatarHeight=60&button=false">

<!--- EndOpenCollectiveBackers -->
<a href="#" id="endofbacking"></a>


## Usage

<!-- snippet: MsLoggingInCode -->
<a id='snippet-msloggingincode'/></a>
```cs
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information);
    loggingBuilder.AddConsole();
});

using var loggerFactory = new LoggerFactory();
var logFactory = LogManager.Use<MicrosoftLogFactory>();
logFactory.UseMsFactory(loggerFactory);
// endpoint startup and shutdown
```
<sup>[snippet source](/src/Tests/Snippets/Usage.cs#L10-L23) / [anchor](#snippet-msloggingincode)</sup>
<!-- endsnippet -->


## Usage when hosting

As `LoggerFactory` implements [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) it must be disposed of after stopping the NServiceBus endpoint. The process for doing this will depend on how the endpoint is being hosted.


### In a windows service

When [hosting in a windows service](/nservicebus/hosting/windows-service.md) `LoggerFactory` should be disposed of as part of the [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx) execution.

<!-- snippet: MsLoggingInService -->
<a id='snippet-mslogginginservice'/></a>
```cs
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
            builder.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information);
            builder.AddConsole();
        });

        var serviceProvider = services.BuildServiceProvider();

        loggerFactory = serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>();
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
<sup>[snippet source](/src/Tests/Snippets/ProgramService.cs#L10-L70) / [anchor](#snippet-mslogginginservice)</sup>
<!-- endsnippet -->


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Abstract](https://thenounproject.com/term/abstract/847344/) designed by [Neha Shinde](https://thenounproject.com/neha.shinde) from [The Noun Project](https://thenounproject.com).
