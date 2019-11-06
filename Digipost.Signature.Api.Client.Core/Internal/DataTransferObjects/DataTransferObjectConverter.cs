using Schemas;

namespace Digipost.Signature.Api.Client.Core.Internal.DataTransferObjects
{
    public static class DataTransferObjectConverter
    {
        public static Error FromDataTransferObject(error error)
        {
            if (error == null)
            {
                return Error.Unknown;
            }

            return new Error
            {
                Type = error.errortype,
                Code = error.errorcode,
                Message = error.errormessage
            };
        }
    }
}
