using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signature
    {
        public Signature(SignerIdentifier signer, XadesReference xadesReference, SignatureStatus signatureStatus, DateTime dateTimeForStatus)
        {
            Signer = signer;
            XadesReference = xadesReference;
            SignatureStatus = signatureStatus;
            DateTimeForStatus = dateTimeForStatus;
        }

        public SignerIdentifier Signer { get; set; }

        public XadesReference XadesReference { get; set; }

        public SignatureStatus SignatureStatus { get; set; }

        public DateTime DateTimeForStatus { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus} (since {DateTimeForStatus}), Signer: {Signer}, XadesReference: {XadesReference}";
        }
    }
}