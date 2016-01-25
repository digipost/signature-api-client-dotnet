using System;
using System.Xml.Serialization;

namespace Digipost.Signature.Api.Client.Core
{
    public class Signer
    {
        public string PersonalIdentificationNumber { get; private set; }

        public Signer(string personalIdentificationNumber)
        {
            PersonalIdentificationNumber = personalIdentificationNumber;
        }

    }
}