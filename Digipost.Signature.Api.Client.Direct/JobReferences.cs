﻿using System;
using Digipost.Signature.Api.Client.Core;

namespace Digipost.Signature.Api.Client.Direct
{
    public class JobReferences
    {
        public JobReferences(Uri confirmation, Uri xades, Uri pades)
        {
            Confirmation = new ConfirmationReference(confirmation);
            Xades = new XadesReference(xades);
            Pades = new PadesReference(pades);
        }

        public ConfirmationReference Confirmation { get; private set; }

        public XadesReference Xades{ get; private set; }

        public PadesReference Pades { get; private set; }

    }
}