using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;

namespace Digipost.Signature.Api.Client.Core
{
    public class Environment
    {
        private Environment(X509Certificate2Collection allowedChainCertificates, Uri url, Uri tokenEndpoint = null)
        {
            AllowedChainCertificates = allowedChainCertificates;
            Url = url;
            TokenEndpoint = tokenEndpoint;
        }

        public Uri Url { get; set; }

        public X509Certificate2Collection AllowedChainCertificates { get; set; }

        public Uri TokenEndpoint { get; set; }

        internal static Environment Localhost => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://localhost:8443")
        );

        internal static Environment DockerLocalhost => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://host.docker.internal:8443")
        );

        internal static Environment Qa => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.qa.signering.posten.no"),
            new Uri("https://midp.qa.digipost.no/oauth2/token")
        );

        internal static Environment Test => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.test.signering.posten.no"),
            new Uri("https://midp.test.digipost.no/oauth2/token")
        );

        public static Environment DifiTest => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.difitest.signering.posten.no"),
            new Uri("https://midp.difitest.digipost.no/oauth2/token")
        );

        public static Environment DifiQa => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.difiqa.signering.posten.no"),
            new Uri("https://midp.qa.digipost.no/oauth2/token")
        );

        public static Environment Production => new Environment(
            CertificateChainUtility.ProduksjonsSertifikater(),
            new Uri("https://api.signering.posten.no"),
            new Uri("https://midp.digipost.no/oauth2/token")
        );

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}
