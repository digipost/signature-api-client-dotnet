using System;
using System.Security.Cryptography.X509Certificates;
using Difi.Felles.Utility;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal static class CertificateValidator
    {
        public static bool IsValidServerCertificate(Sertifikatkjedevalidator sertifikatkjedevalidator, X509Certificate2 certificate, string certificateOrganizationNumber)
        {
            if (certificate == null ||
                !IsIssuedToServerOrganizationNumber(certificate, certificateOrganizationNumber) ||
                !IsActiveCertificate(certificate))
            {
                return false;
            }

            return sertifikatkjedevalidator.ErGyldigResponssertifikat(new X509Certificate2(certificate));
        }

        private static bool IsIssuedToServerOrganizationNumber(X509Certificate certificate, string certificateOrganizationNumber)
        {
            return certificate.Subject.Contains($"SERIALNUMBER={certificateOrganizationNumber}") || certificate.Subject.Contains($"CN={certificateOrganizationNumber}");
        }

        private static bool IsActiveCertificate(X509Certificate certificate)
        {
            return DateTime.Now > DateTime.Parse(certificate.GetEffectiveDateString()) && DateTime.Now < DateTime.Parse(certificate.GetExpirationDateString());
        }
    }
}