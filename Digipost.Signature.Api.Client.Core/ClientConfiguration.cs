using System;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public class ClientConfiguration
    {
        public Uri SignatureServiceRoot { get; internal set; }

        public Sender Sender { get; internal set; }

        public X509Certificate2 Certificate { get; internal set; }

        public ClientConfiguration(Uri signatureServiceRoot, Sender sender, string certificateThumbprint) 
            : this(signatureServiceRoot, sender, ApiClientShared.CertificateUtility.SenderCertificate(certificateThumbprint, Language.English))
        {
        }

        public ClientConfiguration(Uri signatureServiceRoot, Sender sender, X509Certificate2 certificate)
        {
            SignatureServiceRoot = signatureServiceRoot;
            Sender = sender;
            Certificate = certificate;
        }
    }
}
