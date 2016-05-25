---
id: Logging
title: Logging
layout: default
---


<h3 id="loggingrequestflow">Logging request flow</h3>
The client is using Common Logging API for .NET as an abstraction for logging. It is up to the user to implement the API with a logging framework.

<blockquote>Common Logging API is a lightweight infrastructure logging platform that allows developers to focus on the logging requirements instead of the logging tools and required configuration. The Common Logging API abstracts the logging requirements of any project making it easy to swap logging providers.</blockquote>


Enabling logging on level `DEBUG` will output positive results of requests and worse, `WARN` only failed requests or worse, while `ERROR` will only occur on failed requests to create a signature job. These loggers will be under the `Digipost.Signature.Api.Client` namespace. 

<h3 id="log4net">Implementing Log4Net</h3>
1. Install Nuget-package `Common.Logging.Log4Net`. This will install the dependencies `Common.Logging.Core` and `Common.Logging`. Note that the versioning of Log4Net is a bit odd, but Nuget Gallery will reveal that Log4Net 2.0.3 has _Log4net [1.2.13] 2.0.3_ as package name. This means that `Common.Logging.Log4Net1213` is the correct logging adapter.
2. In some cases the adapter may install the wrong version of Log4Net. If installing the adapter for 2.0.3, the version must be upped to this version too.

Complete App.config with the Log4Net adapter installed and a `RollingFileAppender`:
{% highlight xml %}
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <sectionGroup name="common">
      <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
    </sectionGroup>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>

  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter, Common.Logging.Log4net1213">
        <arg key="configType" value="INLINE" />
      </factoryAdapter>
    </logging>
  </common>

   <log4net>
    <appender name="RollingFileAppender" type="log4net.Appender.RollingFileAppender">
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <file value="${AppData}\Digipost\Signature\" />
      <appendToFile value="true" />
      <rollingStyle value="Date" />
      <staticLogFileName value="false" />
      <rollingStyle value="Composite" />
      <param name="maxSizeRollBackups" value="10" />
      <datePattern value="yyyy.MM.dd' signature-api-client-dotnet.log'" />
      <maximumFileSize value="100MB" />
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
      </layout>
    </appender>
   <root>
      <appender-ref ref="RollingFileAppender"/>
    </root>
  </log4net>
</configuration>

{% endhighlight %}

<h3 id="loggingrequestresponse">Logging request and response with Log4Net</h3>

For initial integration and debugging purposes, it can be useful to log the actual request and response going over the wire. This can be enabled by creating a logger with the name `Digipost.Signature.Api.Client.RequestLogger`. See the following example for how to log requests to trace and file:

{% highlight xml %}

 <log4net>
    <logger name="Digipost.Signature.Api.Client.RequestLogger">
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

<h3 id="loggingdocumentbundle">Logging of document bundle</h3>

Logging of document bundle can be enabled via the `ClientConfiguration`:

{% highlight csharp %}

var clientConfiguration = new ClientConfiguration(Environment.DifiQa, "3k 7f 30 dd 05 d3 b7 fc...");
clientConfiguration.EnableDocumentBundleDiskDump("/directory/path/for/bundle/disk/dump");

{% endhighlight %}

<blockquote>
Remember to only set the directory to save the disk dump. A new zip file will be placed there for each created signature job. 
</blockquote>

If you have special needs for the bundle other than just saving it to disk, add your own bundle processor to `ClientConfiguration.DocumentBundleProcessors`.



