using System;
using Common.Logging;
using Common.Logging.Simple;

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
                    DisableBuildServerLogging();
                    return Client.DifiTest;
                }

                return Client.Test;
            }
        }

        private static void DisableBuildServerLogging()
        {
            LogManager.Adapter = new NoOpLoggerFactoryAdapter();
        }

        protected static bool IsOnBuildServer()
        {
            var isOnBuildServer = false;

            const string buildServerUser = "administrator";
            var currentUser = System.Environment.UserName.ToLower();
            var isCurrentUserBuildServer = currentUser.Contains(buildServerUser);
            if (isCurrentUserBuildServer)
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
                case Client.Test:
                    result = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "test"));
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
            DifiQa,
            Test
        }
    }
}