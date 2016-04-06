using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Signature.Api.Client.Core.Asice;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration : IAsiceConfiguration
    {
        /// <param name="environment">The environment which all requests with this instance of the configuration connects to.</param>
        /// <param name="certificateThumbprint">
        ///     The thumbprint of the <see cref="Sender">Senders</see> certificate.  All certificates
        ///     have a thumbprint which uniquely identifies them. If installed in certificate store of current user, the thumbprint
        ///     can
        ///     be used to retreieve it. Remember to add it to the store as  exportable to use it in Signature client.
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
        public Sender GlobalSender { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public string ServerCertificateOrganizationNumber { get; } = "984661185";

        public IEnumerable<IDocumentBundleProcessor> DocumentBundleProcessors { get; set; } = new List<IDocumentBundleProcessor>();

        public void EnableDocumentBundleDiskDump(string directory)
        {
            var documentBundleToDiskProcessor = new DocumentBundleToDiskProcessor(directory);
            ((List<IDocumentBundleProcessor>) DocumentBundleProcessors).Add(documentBundleToDiskProcessor);
        }
    }
}