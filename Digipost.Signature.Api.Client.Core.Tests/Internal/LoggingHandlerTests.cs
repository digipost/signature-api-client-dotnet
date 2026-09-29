using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Internal
{
    public class LoggingHandlerTests
    {
        private class RecordingLogger : ILogger
        {
            public List<string> Messages { get; } = new List<string>();

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Messages.Add(formatter(state, exception));
            }
        }

        private class RecordingLoggerFactory : ILoggerFactory
        {
            public RecordingLogger Logger { get; } = new RecordingLogger();

            public ILogger CreateLogger(string categoryName) => Logger;

            public void AddProvider(ILoggerProvider provider)
            {
            }

            public void Dispose()
            {
            }
        }

        private class FakeInnerHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(string.Empty)});
            }
        }

        public class SendAsync : LoggingHandlerTests
        {
            [Fact]
            public async Task Redacts_authorization_header_when_logging_request()
            {
                //Arrange
                var loggerFactory = new RecordingLoggerFactory();
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetTestCertificate(), CoreDomainUtility.JwtClientId, CoreDomainUtility.GetAccountId(), CoreDomainUtility.GetSender()) {LogRequestAndResponse = true};

                var loggingHandler = new LoggingHandler(clientConfiguration, loggerFactory) {InnerHandler = new FakeInnerHandler()};
                var client = new HttpClient(loggingHandler) {BaseAddress = new Uri("https://api.example.no")};

                var request = new HttpRequestMessage(HttpMethod.Get, "signature-jobs");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "a-very-secret-access-token");

                //Act
                await client.SendAsync(request).ConfigureAwait(false);

                //Assert
                Assert.Contains(loggerFactory.Logger.Messages, message => message.Contains("Authorization: <redacted>"));
                Assert.DoesNotContain(loggerFactory.Logger.Messages, message => message.Contains("a-very-secret-access-token"));
            }
        }
    }
}
