namespace Digipost.Signature.Api.Client.Core.Exceptions
{
    internal class InvalidXmlException : SignatureException
    {
        public InvalidXmlException(string message)
            : base(message)
        {
        }
    }
}
