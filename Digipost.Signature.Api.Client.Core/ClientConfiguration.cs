using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration
    {
        public ClientConfiguration(Environment environment, string certificateThumbprint, Sender sender = null)
            : this(environment, CertificateUtility.SenderCertificate(certificateThumbprint, Language.English), sender)
        {
        }

        public ClientConfiguration(Environment environment, X509Certificate2 certificate, Sender sender = null)
        {
            Environment = environment;
            Sender = sender;
            Certificate = certificate;
        }

        public Environment Environment { get; }

        public Sender Sender { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public string ServerCertificateOrganizationNumber { get; } = "984661185";
    }
}