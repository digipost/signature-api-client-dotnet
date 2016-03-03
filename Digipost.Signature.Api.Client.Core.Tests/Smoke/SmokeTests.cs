using System;

namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class SmokeTests
    {
        protected static Client ClientType
        {
            get
            {
                if (IsOnBuildServer())
                {
                    return Client.DifiQa;
                }

                return Client.DifiTest;
            }
        }

        protected static bool IsOnBuildServer()
        {
            var isOnBuildServer = false;

            const string teamCityBuildUser = "Administrator";
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
                    result = new Uri(Environment.Localhost.Url, relativeUri);
                    break;
                case Client.DifiTest:
                    result = new Uri(Environment.DifiTest.Url, relativeUri);
                    break;
                case Client.DifiQa:
                    result = new Uri(Environment.DifiQa.Url, relativeUri);
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