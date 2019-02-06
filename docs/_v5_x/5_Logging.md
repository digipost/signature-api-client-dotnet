---
identifier: Logging
title: Logging
layout: default
---


### Logging request flow

The client library has the ability to log useful information that can be used for debug purposes. To enable logging, supply the `Direct` or `Portal` client with an implementation of `Microsoft.Extensions.Logging.ILoggerFactory`. This is Microsoft's own logging API, and allows the user to chose their own logging framework.  

Enabling logging on level `DEBUG` will output positive results of requests and worse, `WARN` only failed requests or worse, while `ERROR` will only occur on failed requests to create a signature job. These loggers will be under the `Digipost.Signature.Api.Client` namespace. 

### Implementing using NLog

There are numerous ways to implement a logger, but the following examples will be based on [NLog](https://github.com/NLog/NLog.Extensions.Logging/wiki/Getting-started-with-.NET-Core-2---Console-application) documentation

1. Install the Nuget-packages `NLog`, `NLog.Extensions.Logging` and `Microsoft.Extensions.DependencyInjection`.
1. Create a _nlog.config_ file. The following is an example that logs to file and to console:
```xml

<?xml version="1.0" encoding="utf-8"?>

<!-- XSD manual extracted from package NLog.Schema: https://www.nuget.org/packages/NLog.Schema-->
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xsi:schemaLocation="NLog NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogFile="c:\temp\console-example-internal.log"
      internalLogLevel="Info">
    <!-- the targets to write to -->
    <targets>
        <!-- write logs to file -->
        <target xsi:type="File"
                name="fileTarget"
                fileName="/logs/signature-api-client-dotnet/signature-api-client-dotnet.log"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}"
                archiveEvery="Day"
                archiveNumbering="Date"
                archiveDateFormat="yyyy-MM-dd"/>
        <!-- write logs to console -->
        <target xsi:type="Console"
                name="consoleTarget"
                layout="${date}|${level:uppercase=true}|${message} ${exception}|${logger}|${all-event-properties}" />
    </targets>

    <!-- rules to map from logger name to target -->
    <rules>
        <logger name="*" minlevel="Trace" writeTo="fileTarget,consoleTarget"/>
    </rules>
</nlog>
```
3. In your application, do the following to create a logger and supply it to the client:
```c#
private static IServiceProvider CreateServiceProviderAndSetUpLogging()
{
    var services = new ServiceCollection();

    services.AddSingleton<ILoggerFactory, LoggerFactory>();
    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
    services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

    var serviceProvider = services.BuildServiceProvider();
    SetUpLoggingForTesting(serviceProvider);

    return serviceProvider;
}

private static void SetUpLoggingForTesting(IServiceProvider serviceProvider)
{
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    loggerFactory.AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true});
    NLog.LogManager.LoadConfiguration("./nlog.config");
}

static void Main(string[] args)
{
    ClientConfiguration clientConfiguration = null;
    var serviceProvider = CreateServiceProviderAndSetUpLogging();
    var client = new PortalClient(clientConfiguration, serviceProvider.GetService<ILoggerFactory>());
}

```

### Logging request and response with Log4Net

For initial integration and debugging purposes, it can be useful to log the actual request and response going over the wire. This can be enabled by doing the following:

* Set the property `ClientConfiguration.LogRequestAndResponse = true` and
* Create a logger with the name `Digipost.Signature.Api.Client.RequestResponse`. See the following example for how to log requests to trace and file:

{% highlight xml %}

 <log4net>
    <logger name="Digipost.Signature.Api.Client.RequestResponse">
      <appender-ref ref="TraceAppender"/>
      <appender-ref ref="RollingFileAppender"/>
      <level value="DEBUG"/>
    </logger>
    <appender name="TraceAppender" type="log4net.Appender.TraceAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5p %c %message%newline" />
      </layout>
    </appender>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <file value="${AppData}\Digipost\Signering\RequestLog\" />
      <appendToFile value="true" />
      <rollingStyle value="Date" />
      <staticLogFileName value="false" />
      <rollingStyle value="Composite" />
      <param name="maxSizeRollBackups" value="10" />
      <datePattern value="yyyy.MM.dd' signature-api-client-dotnet.log'" />
      <maximumFFileSize value="100MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
      </layout>
    </appender>
  </log4net>

{% endhighlight %}

<blockquote>
Warning: Enabling request logging should never be used in a production system. It will severely impact the performance of the client.	
</blockquote>

### Logging of document bundle

Logging of document bundle can be enabled via the `ClientConfiguration`:

{% highlight csharp %}

var clientConfiguration = new ClientConfiguration(Environment.DifiTest, "3k 7f 30 dd 05 d3 b7 fc...");
clientConfiguration.EnableDocumentBundleDiskDump("/directory/path/for/bundle/disk/dump");

{% endhighlight %}

<blockquote>
Remember to only set the directory to save the disk dump. A new zip file will be placed there for each created signature job. 
</blockquote>

If you have special needs for the bundle other than just saving it to disk, add your own bundle processor to `ClientConfiguration.DocumentBundleProcessors`.



