using System;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Core.Identifier
{
    public class ContactInformation : SignerIdentifier
    {
        public Email Email { get; internal set; }

        public Sms Sms { get; internal set; }

        public bool IsEmailSpecified => Email != null;

        public bool IsSmsSpecified => Sms != null;

        internal ContactInformation() { }

        internal ContactInformation(notifications notifications)
        {
            foreach (var item in notifications.Items)
            {
                if (item is email && !IsEmailSpecified)
                {
                    Email = new Email(((email)item).address);
                }
                else if (item is sms && !IsSmsSpecified)
                {
                    Sms = new Sms(((sms) item).number);
                }
                else
                {
                    throw new ArgumentException($"Unable to create {nameof(ContactInformation)} from notification elements. Only one of each is allowed.");
                }
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
                return
                    (Email?.GetHashCode() ?? 0) ^
                    (Sms?.GetHashCode() ?? 0);
            }
        }

        public override bool IsSameAs(SignerIdentifier other)
        {
            if (other is ContactInformation otherContactInformation)
            {
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