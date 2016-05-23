using System.Collections.Generic;

namespace Digipost.Signature.Api.Client.Portal
{
    public class SignatureStatus
    {
        public static SignatureStatus Rejected = new SignatureStatus("REJECTED");

        public static SignatureStatus Cancelled = new SignatureStatus("CANCELLED");

        public static SignatureStatus Reserved = new SignatureStatus("RESERVED");

        public static SignatureStatus ContactInformationMissing = new SignatureStatus("CONTACT_INFORMATION_MISSING");

        public static SignatureStatus Expired = new SignatureStatus("EXPIRED");

        public static SignatureStatus Waiting = new SignatureStatus("WAITING");

        public static SignatureStatus Signed = new SignatureStatus("SIGNED");

        public static IEnumerable<SignatureStatus> KnownStatuses = new List<SignatureStatus>
        {
            Rejected,
            Cancelled,
            Reserved,
            ContactInformationMissing,
            Expired,
            Waiting,
            Signed
        };

        public SignatureStatus(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }

        public override string ToString()
        {
            return $"{GetType().Name}: {Identifier}";
        }

        public override bool Equals(object obj)
        {
            var that = obj as SignatureStatus;

            return that != null && Identifier.Equals(that.Identifier);
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}