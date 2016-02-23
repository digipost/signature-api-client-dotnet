using System;

namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class SmokeTests
    {
        protected static readonly Uri Localhost = new Uri("https://172.16.91.1:8443");
        protected static readonly Uri DifitestSigneringPostenNo = new Uri("https://api.difitest.signering.posten.no");
        protected static readonly Uri DifiqaSigneringPostenNo = new Uri("https://api.difiqa.signering.posten.no");

        protected static Client ClientType
        {
            get
            {
                if (IsOnBuildServer())
                {
                    return Client.DifiQa;
                }

                return Client.DifiQa;
            }
        }

        protected static bool IsOnBuildServer()
        {
            var isOnBuildServer = false;

            var teamCityBuildUser = "kapteinen";
            if (System.Environment.UserName.ToLower().Contains(teamCityBuildUser))
            {
                isOnBuildServer = true;
            }

            return isOnBuildServer;
        }

        protected static Uri GetUriFromRelativePath(string relativeUri)
        {
            Uri result;

            switch (ClientType)
            {
                case Client.Localhost:
                    result = new Uri(Localhost, relativeUri);
                    break;
                case Client.DifiTest:
                    result = new Uri(DifitestSigneringPostenNo, relativeUri);
                    break;
                case Client.DifiQa:
                    result = new Uri(DifiqaSigneringPostenNo, relativeUri);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        protected enum Client
        {
            Localhost,
            DifiTest,
            DifiQa
        }
    }
}