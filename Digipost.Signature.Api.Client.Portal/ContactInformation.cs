using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Portal
{
    public class ContactInformation : SignerIdentifier
    {
        public Email Email { get; }

        public Sms Sms { get; }

        public bool IsEmailSpecified => Email != null;

        public bool IsSmsSpecified => Sms != null;

        internal ContactInformation(Email email, Sms sms = null)
        {
            Email = email;
            Sms = sms;
        }

        internal ContactInformation(Sms sms, Email email = null)
        {
            Sms = sms;
            Email = email;
        }

        public IdentifierType Type
        {
            get
            {
                if (IsEmailSpecified && IsSmsSpecified)
                {
                    return IdentifierType.EmailAndSms;
                }

                return IsEmailSpecified ? IdentifierType.Email : IdentifierType.Sms;
            }
        }


        public override bool Equals(object obj)
        {
            var other = obj as ContactInformation;

            return other != null &&
                   other.Sms == Sms &&
                   other.Email == Email;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 17 * 23 + Sms.GetHashCode() + Email.GetHashCode();
            }
        }
    }
}