using System;

namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    /// <summary>
    ///     Thrown when an access token could not be acquired from the configured mIdP token endpoint, or when the
    ///     token endpoint's response could not be understood.
    /// </summary>
    /// <seealso cref="ClientConfiguration" />
    public class AccessTokenException : SecurityException
    {
        public AccessTokenException(string message)
            : base(message)
        {
        }

        public AccessTokenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
