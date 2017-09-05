using System;
using static Digipost.Signature.Api.Client.Core.Environment;
using static Digipost.Signature.Api.Client.Core.Tests.Smoke.SmokeTests;

namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class TestHelperBase
    {
        /// <summary>
        ///     Transforms response Uri to correct environment. This is needed when we develop in order to confirm and cancel and
        ///     get XAdES and PAdES.
        /// </summary>
        /// <returns></returns>
        internal static Uri TransformReferenceToCorrectEnvironment(Uri fullUri)
        {
            return Endpoint == Localhost
                ? new Uri(Localhost.Url, fullUri.AbsolutePath)
                : fullUri;
        }
    }
}