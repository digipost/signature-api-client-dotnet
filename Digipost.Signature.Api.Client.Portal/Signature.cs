using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Signature
    {
        public SignatureStatus SignatureStatus { get; set; }

        public IdentifierType IdentifierType { get; set; }       

        public string Identifier { get; set; }

        public XadesReference XadesReference { get; set; }

        public DateTime DateTimeForStatus { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus} (since {DateTimeForStatus}), Identifier: {Identifier}, XadesReference: {XadesReference}";
        }
    }
}