using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    class DocumentStub : Core.AbstractDocument
    {
        public DocumentStub(string title, string message, string fileName, FileType fileType, byte[] documentBytes)
            : base(title, message, fileName, fileType, documentBytes)
        {
        }

        public DocumentStub(string title, string message, string fileName, FileType fileType, string documentPath)
            : base(title, message, fileName, fileType, documentPath)
        {
        }
    }
}
