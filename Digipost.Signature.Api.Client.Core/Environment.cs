using System;
using System.Security.Cryptography.X509Certificates;
using Digipost.Api.Client.Shared.Certificate;

namespace Digipost.Signature.Api.Client.Core
{
    public class Environment
    {
        private Environment(X509Certificate2Collection allowedChainCertificates, Uri url)
        {
            AllowedChainCertificates = allowedChainCertificates;
            Url = url;
        }

        public Uri Url { get; set; }

        public X509Certificate2Collection AllowedChainCertificates { get; set; }

        internal static Environment Localhost => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://172.16.91.1:8443")
        );

        public static Environment DifiTest => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.difitest.signering.posten.no")
        );

        public static Environment DifiQa => new Environment(
            CertificateChainUtility.FunksjoneltTestmiljøSertifikater(),
            new Uri("https://api.difiqa.signering.posten.no")
        );

        public static Environment Production => new Environment(
            CertificateChainUtility.ProduksjonsSertifikater(),
            new Uri("https://api.signering.posten.no")
        );

        public override string ToString()
        {
            return $"Url: {Url}";
        }
    }
}