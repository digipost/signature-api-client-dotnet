using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Docs.ClientConfigurationExample
{
    public class CreateClientConfiguration
    {
        public static void FromSecrets()
        {
            const string organizationNumber = "123456789";

            var clientConfiguration = new ClientConfiguration(
                Environment.DifiTest,
                CertificateReader.ReadCertificate(),
                new Sender(organizationNumber));
        }

        public static void FromThumbprint()
        {
            const string organizationNumber = "123456789";
            const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

            var clientConfiguration = new ClientConfiguration(
                Environment.DifiTest,
                certificateThumbprint,
                new Sender(organizationNumber));
        }
    }
}
