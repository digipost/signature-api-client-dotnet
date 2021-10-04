using System;
using System.Net.Http;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class UserAgentHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("User-Agent", GetInformationalVersion());

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        
        private static string GetInformationalVersion()
        {
            var informationalVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            return $"posten-signature-api-client-dotnet/{informationalVersion} (netcore/{GetNetCoreVersion()})";
        }

        private static string GetNetCoreVersion()
        {
            try
            {
                var assembly = typeof(GCSettings).GetTypeInfo().Assembly;
                var assemblyPath = assembly.CodeBase.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                var netCoreAppIndex = Array.IndexOf(assemblyPath, "Microsoft.NETCore.App");

                if (netCoreAppIndex > 0 && netCoreAppIndex < assemblyPath.Length - 2)
                {
                    return assemblyPath[netCoreAppIndex + 1];
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return "AssemblyVersionNotFound";
        }
    }
}
