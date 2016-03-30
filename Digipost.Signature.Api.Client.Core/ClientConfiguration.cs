using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Digipost.Signature.Api.Client.Core.Asice;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration : IAsiceConfiguration
    {
        public ClientConfiguration(Environment environment, string certificateThumbprint, Sender globalSender = null)
            : this(environment, CertificateUtility.SenderCertificate(certificateThumbprint, Language.English), globalSender)
        {
        }

        public ClientConfiguration(Environment environment, X509Certificate2 certificate, Sender globalSender = null)
        {
            Environment = environment;
            GlobalSender = globalSender;
            Certificate = certificate;
        }

        public Environment Environment { get; }

        public Sender GlobalSender { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public string ServerCertificateOrganizationNumber { get; } = "984661185";

        public IEnumerable<IDocumentBundleProcessor> DocumentBundleProcessors { get; set; } = new List<IDocumentBundleProcessor>();

        public void EnableDocumentBundleDiskDump(string directory)
        {
            var documentBundleToDiskProcessor = new DocumentBundleToDiskProcessor(directory);

            var hasDocumentBundleProcessor = DocumentBundleProcessors.Any(p => p.GetType() == typeof (DocumentBundleToDiskProcessor));
            if (!hasDocumentBundleProcessor)
            {
                ((List<IDocumentBundleProcessor>) DocumentBundleProcessors).Add(documentBundleToDiskProcessor);
            }
        }
    }
}