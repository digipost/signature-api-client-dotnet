namespace Digipost.Signature.Api.Client.Core
{
    public class CertificateValidationPreferences
    {
        public bool ValidateSenderCertificate { get; set; } = true;

        public bool ValidateResponseCertificate { get; set; } = true;
    }
}
