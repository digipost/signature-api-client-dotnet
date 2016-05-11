using System.Collections;
using System.Collections.Generic;

namespace Digipost.Signature.Api.Client.Portal.Enums
{
    public class SignatureStatus
    {
        public string Identifier { get; }

        public static SignatureStatus Rejected = new SignatureStatus("REJECTED");

        public static SignatureStatus Cancelled = new SignatureStatus("CANCELLED");

        public static SignatureStatus Reserved = new SignatureStatus("RESERVED");

        public static SignatureStatus ContactInformationMissing = new SignatureStatus("CONTACT_INFORMATION_MISSING");

        public static SignatureStatus Expired = new SignatureStatus("EXPIRED");

        public static SignatureStatus Waiting = new SignatureStatus("WAITING");

        public static SignatureStatus Signed = new SignatureStatus("SIGNED");
        
        public static IEnumerable<SignatureStatus> KnownStatuses = new List<SignatureStatus>()
        {
            Rejected,
            Cancelled,
            Reserved,
            ContactInformationMissing,
            Expired,
            Waiting,
            
        };

        public SignatureStatus(string identifier)
        {
            Identifier = identifier;
        }
    }
}