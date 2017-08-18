using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signature
    {
        internal Signature()
        {
        }

        internal Signature(signature signature)
        {
            SignatureStatus = new SignatureStatus(signature.status.Value);
            XadesReference = signature.xadesurl == null ? null : new XadesReference(new Uri(signature.xadesurl));
            DateTimeForStatus = signature.status.since;

            if (signature.Item is notifications)
            {
                Identifier = new ContactInformation((notifications) signature.Item);
            }
            else if (signature.Item is string)
            {
                Identifier = new PersonalIdentificationNumber((string) signature.Item);
            }
        }

        public SignatureStatus SignatureStatus { get; set; }

        public SignerIdentifier Identifier { get; set; }

        public XadesReference XadesReference { get; set; }

        public DateTime DateTimeForStatus { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus} (since {DateTimeForStatus}), Identifier: {Identifier}, XadesReference: {XadesReference}";
        }
    }
}