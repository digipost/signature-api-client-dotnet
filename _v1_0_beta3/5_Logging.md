---
id: Logging
title: Logging
layout: default
---


<h3 id="loggingrequestflow">Logging request flow</h3>
The client has multiple logging options. The logging framework used is Log4Net 2.0.4. Be sure to use 2.0.4 or above if you want to enable logging.

Enabling logging on level `DEBUG` will output positive results of requests and worse, `WARN` only failed requests or worse, while `ERROR` will only occur on failed requests to create a signature job. These loggers will be under the `Digipost.Signature.Api.Client` namespace. 

<h3 id="loggingrequestresponse">Logging request and response</h3>

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
