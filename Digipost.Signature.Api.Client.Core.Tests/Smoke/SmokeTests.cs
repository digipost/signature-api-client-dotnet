using System;

namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class SmokeTests
    {
        internal static Client ClientType
        {
            get
            {
                if (IsOnBuildServer())
                {
                    return Client.Qa;
                }

                return Client.Qa;
            }
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

        internal static Uri GetUriFromRelativePath(string relativeUri)
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
                    var testUri = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "test"));
                    result = new Uri(testUri, relativeUri);
                    break;
                case Client.Qa:
                    var qaUri = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "qa"));
                    result = new Uri(qaUri, relativeUri);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }

        internal enum Client
        {
            Localhost,
            DifiTest,
            DifiQa,
            Test,
            Qa
        }
    }
}