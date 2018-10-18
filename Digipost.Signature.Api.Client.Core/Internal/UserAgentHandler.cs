using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class UserAgentHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("User-Agent", GetAssemblyVersion());

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static string GetAssemblyVersion()
        {
//            var netVersion = Assembly
//                .GetExecutingAssembly()
//                .GetReferencedAssemblies().First(x => x.Name == "netstandard").Version.ToString();

            var netVersion = "2.1.0";

            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return $"digipost-signature-api-client-dotnet/{assemblyVersion} (netcore/{netVersion})";
        }
    }
}
