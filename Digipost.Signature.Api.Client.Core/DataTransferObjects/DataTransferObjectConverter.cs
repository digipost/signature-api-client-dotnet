using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Core.DataTransferObjects
{
    public static class DataTransferObjectConverter
    {
        public static Error FromDataTransferObject(error error)
        {
            return new Error
            {
                Type = error.errortype,
                Code = error.errorcode,
                Message = error.errormessage
            };
        }
    }
}
