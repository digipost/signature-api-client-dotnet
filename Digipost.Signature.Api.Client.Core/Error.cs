namespace Digipost.Signature.Api.Client.Core
{
    public class Error
    {
        public static readonly Error unknown = new Error(){ Code = "Unknown", Message = "Unknown", Type = "Unknown" };
     
        public string Code { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return $"Code: {Code}, Type: {Type}, Message: {Message}";
        }
    }
}
