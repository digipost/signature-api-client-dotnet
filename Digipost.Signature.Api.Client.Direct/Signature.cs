using Digipost.Signature.Api.Client.Core;
using System;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signature
    {
        public Signature(PersonalIdentificationNumber signer, XadesReference xadesReference, SignatureStatus signatureStatus, DateTime dateTimeForStatus)
        {
            Signer = signer;
            XadesReference = xadesReference;
            SignatureStatus = signatureStatus;
            DateTimeForStatus = dateTimeForStatus;
        }

        public PersonalIdentificationNumber Signer { get; set; }

        public XadesReference XadesReference { get; set; }

        public SignatureStatus SignatureStatus { get; set; }

        public DateTime DateTimeForStatus { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus} (since {DateTimeForStatus}), Signer: {Signer}, XadesReference: {XadesReference}";
        }
    }
}