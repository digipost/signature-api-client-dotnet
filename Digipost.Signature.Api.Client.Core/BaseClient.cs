using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Common.Logging;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Api.Client.Shared.Resources.Language;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public abstract class BaseClient
    {
        protected const int TooManyRequestsStatusCode = 429;
        protected const string NextPermittedPollTimeHeader = "X-Next-permitted-poll-time";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private HttpClient _httpClient;

        protected BaseClient(ClientConfiguration clientConfiguration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ClientConfiguration = clientConfiguration;
            HttpClient = MutualTlsClient();
            RequestHelper = new RequestHelper(HttpClient);
            SetMessageLanguageForDigipostApiClientShared();
        }

        public ClientConfiguration ClientConfiguration { get; }

        internal HttpClient HttpClient
        {
            get { return _httpClient; }
            set
            {
                _httpClient = value;
                RequestHelper = new RequestHelper(value);
            }
        }

        internal RequestHelper RequestHelper { get; set; }

        protected Sender CurrentSender(Sender jobSender)
        {
            var sender = jobSender ?? ClientConfiguration.GlobalSender;
            if (sender == null)
            {
                throw new SenderNotSpecifiedException();
            }

            if (!ClientConfiguration.CertificateValidationPreferences.ValidateSenderCertificate)
            {
                Log.Warn($"Validation of {nameof(Sender)} certificate is disabled and should only be disabled under special circumstances. This validation is in place to give a better descriptions in case of an invalid sender certificate.");

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
                throw new CertificateException($"Sertificate used for {nameof(sender)} is not valid. Are you trying to use a test certificate in a production environment or the other way around? The reason is '{validationResult.Type}', description: '{validationResult.Message}'", null);
            }
        }

        private HttpClient MutualTlsClient()
        {
            var client = HttpClientFactory.Create(
                MutualTlsHandler(),
                new XsdRequestValidationHandler(),
                new UserAgentHandler(),
                new LoggingHandler(ClientConfiguration)
            );

            client.Timeout = TimeSpan.FromMilliseconds(ClientConfiguration.HttpClientTimeoutInMilliseconds);
            client.BaseAddress = ClientConfiguration.Environment.Url;

            return client;
        }

        private WebRequestHandler MutualTlsHandler()
        {
            var certificateCollection = new X509Certificate2Collection {ClientConfiguration.Certificate};
            var mutualTlsHandler = new WebRequestHandler();
            mutualTlsHandler.ClientCertificates.AddRange(certificateCollection);
            mutualTlsHandler.ServerCertificateValidationCallback = ValidateServerCertificateThrowIfInvalid;

            return mutualTlsHandler;
        }

        private bool ValidateServerCertificateThrowIfInvalid(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            if (!ClientConfiguration.CertificateValidationPreferences.ValidateResponseCertificate)
            {
                Log.Warn("Validation of response certificate is disabled and should only be disabled under special circumstances. This validation is in place to ensure that the response is from the server you are expecting.");
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

        private static void SetMessageLanguageForDigipostApiClientShared()
        {
            LanguageResource.CurrentLanguage = Language.English;
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