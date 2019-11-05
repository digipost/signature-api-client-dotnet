﻿using System.Net;
using Schemas;

namespace Digipost.Signature.Api.Client.Core.Internal.DataTransferObjects
{
    public static class DataTransferObjectConverter
    {
        public static Error FromDataTransferObject(error error, HttpStatusCode code)
        {
            return new Error
            {
                Type = error == null ? "Content-Type: Unknown" : error.errortype,
                Code = error == null ? code.ToString() : error.errorcode,
                Message = error == null ? "Unknown message" : error.errormessage
            };
        }
    }
}
