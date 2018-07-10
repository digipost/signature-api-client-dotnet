using System.Collections.Generic;

namespace Digipost.Signature.Api.Client.Direct
{
    public class SignatureStatus
    {
        public static SignatureStatus Rejected = new SignatureStatus("REJECTED");

        public static SignatureStatus Failed = new SignatureStatus("FAILED");

        public static SignatureStatus Expired = new SignatureStatus("EXPIRED");

        public static SignatureStatus Waiting = new SignatureStatus("WAITING");

        public static SignatureStatus Signed = new SignatureStatus("SIGNED");

        public static IEnumerable<SignatureStatus> KnownStatuses = new List<SignatureStatus>
        {
            Rejected,
            Failed,
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
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Identifier.GetHashCode();
                return hash;
            }
        }
    }
}