using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobReferences
    {
        public JobReferences(Uri confirmation, Uri pades)
        {
            Confirmation = new ConfirmationReference(confirmation);
            Pades = new PadesReference(pades);
        }

        public ConfirmationReference Confirmation { get; internal set; }

        public PadesReference Pades { get; internal set; }

        public override string ToString()
        {
            return $"Confirmation: {Confirmation}, Pades: {Pades}";
        }
    }
}