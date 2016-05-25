using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
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
        public ClientConfiguration(Environment environment, string certificateThumbprint, Sender globalSender = null)
            : this(environment, CertificateUtility.SenderCertificate(certificateThumbprint, Language.English), globalSender)
        {
        }

        /// <param name="environment">The environment which all requests with this instance of the configuration connects to.</param>
        /// <param name="certificate">Certificate of the <see cref="Sender" />.</param>
        /// <param name="globalSender">
        ///     If set, it will be used for all <see cref="ISignatureJob">SignatureJobs</see> created without a
        ///     <see cref="Sender" />.
        /// </param>
        public ClientConfiguration(Environment environment, X509Certificate2 certificate, Sender globalSender = null)
        {
            Environment = environment;
            GlobalSender = globalSender;
            Certificate = certificate;
        }

        public Environment Environment { get; }

        /// <summary>
        ///     If set, it will be used for all <see cref="ISignatureJob">SignatureJobs</see> created without
        ///     a <see cref="Sender" />.
        /// </summary>
        public Sender GlobalSender { get; set; }

        public X509Certificate2 Certificate { get; internal set; }

        public int HttpClientTimeoutInMilliseconds { get; set; } = 3000;

        public string ServerCertificateOrganizationNumber { get; } = "984661185";

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
    }
}