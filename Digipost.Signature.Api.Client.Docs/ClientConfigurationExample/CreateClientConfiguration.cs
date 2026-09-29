using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Docs.ClientConfigurationExample
{
    public class CreateClientConfiguration
    {
        /// <summary>
        ///     The certificate is used to sign the document bundle and to authenticate against mIdP using JWT/mTLS
        ///     authentication - it is not presented on every request to the Signature API, an access token acquired
        ///     from mIdP is used instead.
        /// </summary>
        public static void FromSecrets()
        {
            const string organizationNumber = "123456789";
            const string clientId = "your-client-id";
            const string accountId = "your-account-id";

            var clientConfiguration = new ClientConfiguration(
                Environment.DifiTest,
                CertificateReader.ReadCertificate(),
                clientId,
                new AccountId(accountId),
                new Sender(organizationNumber));
        }

        public static void FromThumbprint()
        {
            const string organizationNumber = "123456789";
            const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";
            const string clientId = "your-client-id";
            const string accountId = "your-account-id";

            var clientConfiguration = new ClientConfiguration(
                Environment.DifiTest,
                certificateThumbprint,
                clientId,
                new AccountId(accountId),
                new Sender(organizationNumber));
        }
    }
}
