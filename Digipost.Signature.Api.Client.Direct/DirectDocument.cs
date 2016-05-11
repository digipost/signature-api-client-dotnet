using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class DirectDocument : Document
    {
        public DirectDocument(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
            : base(title, message, fileName, fileType, documentBytes)
        {
        }

        public DirectDocument(string title, string message, string fileName, FileType fileType, string documentPath)
            : base(title, message, fileName, fileType, documentPath)
        {
        }
    }
}
