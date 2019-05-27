using System;
using Digipost.Signature.Api.Client.Core;
using Schemas;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Signature
    {
        internal Signature(signerstatus status, signerspecificurl xadesUrl)
        {
            Signer = status.signer;
            SignatureStatus = new SignatureStatus(status.signer);
            XadesReference = xadesUrl == null ? null : new XadesReference(new Uri(xadesUrl.Value));
            SignatureStatus = new SignatureStatus(status.Value);
            DateTimeForStatus = status.since;
        }

        public Signature(string signer, XadesReference xadesReference, SignatureStatus signatureStatus, DateTime dateTimeForStatus)
        {
            Signer = signer;
            XadesReference = xadesReference;
            SignatureStatus = signatureStatus;
            DateTimeForStatus = dateTimeForStatus;
        }

        public string Signer { get; set; }

        public XadesReference XadesReference { get; set; }

        public SignatureStatus SignatureStatus { get; set; }

        public DateTime DateTimeForStatus { get; set; }

        public override string ToString()
        {
            return $"SignatureStatus: {SignatureStatus} (since {DateTimeForStatus}), Signer: {Signer}, XadesReference: {XadesReference}";
        }
    }
}
