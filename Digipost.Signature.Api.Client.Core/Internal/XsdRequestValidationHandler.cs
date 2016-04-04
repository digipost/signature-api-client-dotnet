using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Difi.Felles.Utility;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Xsd;
using log4net;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class XsdRequestValidationHandler : XsdValidationHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ValidateXmlAndThrowIfInvalid(request.Content, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}