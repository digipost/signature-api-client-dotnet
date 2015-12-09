using System;
using ApiClientShared;

namespace Digipost.Signature.Api.Client.Core
{
    public static class DomainUtility
    {
        static ResourceUtility resourceUtility = new ResourceUtility("Digipost.Signature.Api.Client.CoreTests.Resources.Documents");

        public static byte[] GetPdfDocumentBytes()
        {
            return resourceUtility.ReadAllBytes(true, "Dokument.pdf");
        }
    }
}
