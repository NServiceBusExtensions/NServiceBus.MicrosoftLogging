# <img src="/src/icon.png" height="30px"> NServiceBus.MicrosoftLogging

[![Build status](https://ci.appveyor.com/api/projects/status/sovlo1pvgfh0xnba/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/nservicebus-MicrosoftLogging)
[![NuGet Status](https://img.shields.io/nuget/v/NServiceBus.MicrosoftLogging.svg)](https://www.nuget.org/packages/NServiceBus.MicrosoftLogging/)

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

snippet: MsLoggingInCode


## Usage when hosting

As `LoggerFactory` implements [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) it must be disposed of after stopping the NServiceBus endpoint. The process for doing this will depend on how the endpoint is being hosted.


### In a generic host

Disposing the `LoggerFactory` is done by the underlying infrastructure.

snippet: MsLoggingInGenericHost


### In a windows service

When [hosting in a windows service](https://docs.particular.net/nservicebus/hosting/windows-service) `LoggerFactory` should be disposed of as part of the [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx) execution.

snippet: MsLoggingInService


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Abstract](https://thenounproject.com/term/abstract/847344/) designed by [Neha Shinde](https://thenounproject.com/neha.shinde) from [The Noun Project](https://thenounproject.com).