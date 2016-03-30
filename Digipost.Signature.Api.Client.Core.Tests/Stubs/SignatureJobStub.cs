using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Tests.Stubs
{
    public class SignatureJobStub : ISignatureJob
    {
        public Sender Sender { get; }

        public Document Document { get; }

        public string Reference { get; set; }

    }
}
