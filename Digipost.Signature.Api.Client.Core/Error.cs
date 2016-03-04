namespace Digipost.Signature.Api.Client.Core
{
    public class Error
    {
        public string Code { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return $"Code: {Code}, Type: {Type}, Message: {Message}";
        }
    }
}