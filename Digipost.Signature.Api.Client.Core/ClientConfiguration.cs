using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal.Asice;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration : IAsiceConfiguration
    {
        /// <param name="environment">The environment which all requests with this instance of the configuration connects to.</param>
        /// <param name="certificateThumbprint">
        ///     The thumbprint of the <see cref="Sender">Senders</see> certificate.  All certificates
        ///     have a thumbprint which uniquely identifies them. If installed in certificate store of current user, the thumbprint
        ///     can be used to retreieve it. Remember to add it to the store as  exportable to use it in Signature client.
        /// </param>
        /// <param name="globalSender">
        ///     If set, it will be used for all <see cref="ISignatureJob">SignatureJobs</see> created without
        ///     a <see cref="Sender" />.
        /// </param>
        /// <param name="proxy">
        ///     If set, the proxy will be used for all requests. Remember to set <see cref="Credential" /> as well.
        /// </param>
        /// <param name="credential">
        ///     Will be used if both this and <see cref="WebProxy" /> is set.
        /// </param>
        public ClientConfiguration(Environment environment, string certificateThumbprint, Sender globalSender = null, WebProxy proxy = null, NetworkCredential credential = null)
            : this(environment, CertificateUtility.SenderCertificate(certificateThumbprint), globalSender, proxy, credential)
        {
        }

        /// <param name="environment">The environment which all requests with this instance of the configuration connects to.</param>
        /// <param name="certificate">Certificate of the <see cref="Sender" />.</param>
        /// <param name="globalSender">
        ///     If set, it will be used for all <see cref="ISignatureJob">SignatureJobs</see> created without a
        ///     <see cref="Sender" />.
        /// </param>
        /// <param name="proxy">
        ///     If set, the proxy will be used for all requests. Remember to set <see cref="Credential" /> as well.
        /// </param>
        /// <param name="credential">
        ///     Will be used if both this and <see cref="WebProxy" /> is set.
        /// </param>
        public ClientConfiguration(Environment environment, X509Certificate2 certificate, Sender globalSender = null, WebProxy proxy = null, NetworkCredential credential = null)
        {
            Environment = environment;
            GlobalSender = globalSender;
            Certificate = certificate;
            WebProxy = proxy;
            Credential = credential;
        }

        public Environment Environment { get; }

        /// <summary>
        ///     If set, it will be used for all <see cref="ISignatureJob">SignatureJobs</see> created without
        ///     a <see cref="Sender" />.
        /// </summary>
        public Sender GlobalSender { get; set; }

        public X509Certificate2 Certificate { get; set; }

        /// <summary>
        ///     If set, the proxy will be used for all requests. Remember to set <see cref="Credential" /> as well.
        /// </summary>
        public WebProxy WebProxy { get; set; }

        /// <summary>
        ///     Will be used if both this and <see cref="WebProxy" /> is set.
        /// </summary>
        public NetworkCredential Credential { get; set; }

        public int HttpClientTimeoutInMilliseconds { get; set; } = 10000;

        public string ServerCertificateOrganizationNumber { get; } = "984661185";

        /// <summary>
        ///     Set to true to enable request and response logging with level DEBUG
        /// </summary>
        public bool LogRequestAndResponse { get; set; } = false;

        /// <summary>
        /// Preferences used for enabling and disabling validation of certificates used in the client
        /// </summary>
        public CertificateValidationPreferences CertificateValidationPreferences { get; } = new CertificateValidationPreferences();

        /// <summary>
        ///     All bundle processors used for processing document bundle zip files before they are sent to the service to create
        ///     signature jobs. Add a <see cref="IDocumentBundleProcessor">DocumentBundleProcessor</see> here if requirements are
        ///     more specific than what can be achieved from <see cref="EnableDocumentBundleDiskDump" />
        /// </summary>
        public IEnumerable<IDocumentBundleProcessor> DocumentBundleProcessors { get; set; } = new List<IDocumentBundleProcessor>();

        public override string ToString()
        {
            return $"Environment: {Environment}, GlobalSender: {GlobalSender}, Certificate: {Certificate.Subject}, HttpClientTimeoutInMilliseconds: {HttpClientTimeoutInMilliseconds}, ServerCertificateOrganizationNumber: {ServerCertificateOrganizationNumber}";
        }

        /// <summary>
        ///     Have the library dump the generated document bundle zip files to disk before they are sent to the service to create
        ///     signature jobs.
        /// </summary>
        /// <param name="directory">
        ///     The directory in which the bundles will be persisted. The file format will as follows:
        ///     <code>[timestamp] - [reference-from-job].asice.zip</code>
        /// </param>
        public void EnableDocumentBundleDiskDump(string directory)
        {
            var documentBundleToDiskProcessor = new DocumentBundleToDiskProcessor(directory);
            ((List<IDocumentBundleProcessor>) DocumentBundleProcessors).Add(documentBundleToDiskProcessor);
        }


        /// <summary>
        ///     If the client receives a 429 response code and has a header given by this variable, it is blocked by the
        ///     DoS filter. When this happens, it is used together with <see cref="DosFilterBlockingPeriod"/> to give a
        ///     <see cref="TooEagerPollingException"/> with a Next permitted poll time. 
        /// </summary>
        public string DosFilterHeaderBlockingKey { get; set; } = "DoSFilter";

        /// <summary>
        ///     When the client receives a 429 response code, indicating there is a <see cref="TooEagerPollingException"/>,
        ///     the current Organization number will be blocked for a defined time span. If blocked by the DoS-filter, the
        ///     response message does not have a body or indicate next permitted poll time. The Next permitted poll
        ///     time is added with a predefined time, given by this blocking period.
        /// </summary>
        public TimeSpan DosFilterBlockingPeriod { get; set; } = TimeSpan.FromSeconds(30);
    }
}
