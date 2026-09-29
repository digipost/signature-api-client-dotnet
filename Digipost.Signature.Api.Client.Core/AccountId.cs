using System;

namespace Digipost.Signature.Api.Client.Core
{
    /// <summary>
    ///     The Signering-internal account ID for the organization authenticating with <see cref="ClientConfiguration">JWT/mTLS authentication</see>.
    ///     Can be found on the client details page in Nyva, together with the <see cref="ClientConfiguration.ClientId" />, and used to build the OAuth 2.0 <c>scope</c>
    ///     requested from mIdP.
    /// </summary>
    /// <remarks>
    ///     This is <em>not</em> an organization number, but is tied to a specific organization number, and not the same as a <see cref="Sender" />.
    /// </remarks>
    public sealed class AccountId
    {
        public AccountId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The account id must not be blank", nameof(value));
            }

            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return obj is AccountId other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
