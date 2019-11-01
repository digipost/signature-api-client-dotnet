using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Enums;
using Microsoft.Extensions.Logging;
using Schemas;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class BaseClient
    {
        protected const int TooManyRequestsStatusCode = 429;
        public static string NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";

        private HttpClient _httpClient;
        private readonly ILogger<BaseClient> _logger;
        private readonly ILoggerFactory _loggerFactory;

        protected BaseClient(ClientConfiguration clientConfiguration, ILoggerFactory loggerFactory, WebProxy proxy = null, NetworkCredential credential = null)
        {
            _logger = loggerFactory.CreateLogger<BaseClient>();
            _loggerFactory = loggerFactory;

            ClientConfiguration = clientConfiguration;
            HttpClient = MutualTlsClient(proxy, credential);
            RequestHelper = new RequestHelper(HttpClient, _loggerFactory);
        }

        public ClientConfiguration ClientConfiguration { get; }

        internal HttpClient HttpClient
        {
            get => _httpClient;
            set
            {
                _httpClient = value;
                RequestHelper = new RequestHelper(value, _loggerFactory);
            }
        }

        internal RequestHelper RequestHelper { get; private set; }

        protected Sender CurrentSender(Sender jobSender)
        {
            var sender = jobSender ?? ClientConfiguration.GlobalSender;
            if (sender == null)
            {
                throw new SenderNotSpecifiedException();
            }

            if (!ClientConfiguration.CertificateValidationPreferences.ValidateSenderCertificate)
            {
                _logger.LogWarning($"Validation of {nameof(Sender)} certificate is disabled and should only be disabled under special circumstances. This validation is in place to give a better descriptions in case of an invalid sender certificate.");

                return sender;
            }

            ValidateSenderCertificateThrowIfInvalid();

            return sender;
        }

        private void ValidateSenderCertificateThrowIfInvalid()
        {
            var validationResult = CertificateValidator.ValidateCertificateAndChain(ClientConfiguration.Certificate, ClientConfiguration.Environment.AllowedChainCertificates);

            if (validationResult.Type != CertificateValidationType.Valid)
            {
                throw new CertificateException($"Certificate used for {nameof(sender)} is not valid. Are you trying to use a test certificate in a production environment or the other way around? The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }
        }

        private HttpClient MutualTlsClient(WebProxy proxy = null, NetworkCredential credential = null)
        {
            var client = HttpClientFactory.Create(
                MutualTlsHandler(proxy, credential),
                new XsdRequestValidationHandler(),
                new UserAgentHandler(),
                new LoggingHandler(ClientConfiguration, _loggerFactory)
            );

            client.Timeout = TimeSpan.FromMilliseconds(ClientConfiguration.HttpClientTimeoutInMilliseconds);
            client.BaseAddress = ClientConfiguration.Environment.Url;

            return client;
        }

        private HttpClientHandler MutualTlsHandler(WebProxy proxy = null, NetworkCredential credential = null)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (proxy != null)
            {
                proxy.Credentials = credential;
                handler.Proxy = proxy;
                handler.UseProxy = true;
                handler.UseDefaultCredentials = false;
            }
            var clientCertificates = new X509Certificate2Collection { ClientConfiguration.Certificate };
            handler.ClientCertificates.AddRange(clientCertificates);
            handler.ServerCertificateCustomValidationCallback = ValidateServerCertificateThrowIfInvalid;

            return handler;
        }

        private bool ValidateServerCertificateThrowIfInvalid(HttpRequestMessage message, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            if (!ClientConfiguration.CertificateValidationPreferences.ValidateResponseCertificate)
            {
                _logger.LogWarning("Validation of response certificate is disabled and should only be disabled under special circumstances. This validation is in place to ensure that the response is from the server you are  expecting.");
                return true;
            }

            var x509Certificate2 = new X509Certificate2(certificate);

            var validationResult = CertificateValidator.ValidateCertificateAndChain(x509Certificate2, ClientConfiguration.ServerCertificateOrganizationNumber, ClientConfiguration.Environment.AllowedChainCertificates);

            if (validationResult.Type != CertificateValidationType.Valid)
            {
                throw new SecurityException($"Certificate received in the response is not valid. The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }

            return true;
        }

        internal static Uri RelativeUrl(Sender sender, JobType jobType, HttpMethod httpMethod)
        {
            var relativeUri = $"/api/{sender.OrganizationNumber}/{jobType.ToString().ToLower()}/signature-jobs";

            var shouldAppendQueryParameter = httpMethod == HttpMethod.Get;
            var pollingQueueDefined = !string.IsNullOrEmpty(sender.PollingQueue.Name);

            if (shouldAppendQueryParameter && pollingQueueDefined)
            {
                relativeUri += $"?{PollingQueue.QueryParameterName}={sender.PollingQueue.Name}";
            }

            return new Uri(relativeUri, UriKind.Relative);
        }
    }
}
