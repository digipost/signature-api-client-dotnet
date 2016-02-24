using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration
    {
        public ClientConfiguration(Environment environment, Sender sender, string certificateThumbprint)
            : this(environment, sender, CertificateUtility.SenderCertificate(certificateThumbprint, Language.English))
        {
        }

        public ClientConfiguration(Environment environment, Sender sender, X509Certificate2 certificate)
        {
            Environment = environment;
            Sender = sender;
            Certificate = certificate;
            
        }

        public Environment Environment { get; }

        public Sender Sender { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public string ServerCertificateOrganizationNumber { get; } = "SERIALNUMBER=984661185";
    }
}