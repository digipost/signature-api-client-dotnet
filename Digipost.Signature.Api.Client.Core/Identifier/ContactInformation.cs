using System;

namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class ContactInformation : SignerIdentifier
    {
        public ContactInformation()
        {
        }

        internal ContactInformation(notifications notifications)
        {
            foreach (var item in notifications.Items)
            {
                if (item is email && !IsEmailSpecified)
                {
                    Email = new Email(((email) item).address);
                }
                else if (item is sms && !IsSmsSpecified)
                {
                    var number = ((sms) item).number;
                    Sms = new Sms(number);
                }
                else
                {
                    throw new ArgumentException($"Unable to create {nameof(ContactInformation)} from notification elements. Only one of each is allowed.");
                }
            }
        }

        public Email Email { get; set; }

        public Sms Sms { get; set; }

        public bool IsEmailSpecified => Email != null;

        public bool IsSmsSpecified => Sms != null;

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
                return
                    (Email?.GetHashCode() ?? 0) ^
                    (Sms?.GetHashCode() ?? 0);
            }
        }

        public override bool IsSameAs(SignerIdentifier other)
        {
            if (other is ContactInformation)
            {
                var otherContactInformation = (ContactInformation) other;
                return IsEqual(Sms, otherContactInformation.Sms)
                       && IsEqual(Email, otherContactInformation.Email);
            }

            return false;
        }

        public override string ToString()
        {
            if (IsEmailSpecified && IsSmsSpecified)
            {
                return $"{nameof(ContactInformation)} with {nameof(Sms).ToLower()} {Sms} and {nameof(Email).ToLower()} {Email}";
            }

            return IsEmailSpecified
                ? $"{nameof(ContactInformation)} with {nameof(Email).ToLower()} {Email}"
                : $"{nameof(ContactInformation)} with {nameof(Sms).ToLower()} {Sms}";
        }
    }
}
