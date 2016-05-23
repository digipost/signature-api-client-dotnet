using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class XsdRequestValidationHandler : XsdValidationHandler
    {
        private const string MultipartMixed = "multipart/mixed";
        private const string ApplicationOctetStream = "application/octet-stream";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await ValidateAndThrowIfInvalid(request.Content, cancellationToken);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task ValidateAndThrowIfInvalid(HttpContent content, CancellationToken cancellationToken)
        {
            var contentMediaType = content?.Headers.ContentType?.MediaType;

            if (contentMediaType == MultipartMixed)
            {
                var multipart = await content.ReadAsMultipartAsync(cancellationToken);

                foreach (var httpContent in multipart.Contents)
                {
                    switch (httpContent.Headers.ContentType.MediaType)
                    {
                        case ApplicationXml:
                            ValidateXmlAndThrowIfInvalid(await httpContent.ReadAsStringAsync());
                            break;
                        case ApplicationOctetStream:
                            ValidateByteStreamAndThrowIfInvalid(await httpContent.ReadAsByteArrayAsync());
                            break;
                    }
                }
            }
        }

        private static void ValidateByteStreamAndThrowIfInvalid(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
            {
                foreach (var zipArchiveEntry in archive.Entries)
                {
                    if (zipArchiveEntry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var stream = zipArchiveEntry.Open())
                        using (var reader = new StreamReader(stream))
                        {
                            var result = reader.ReadToEnd();
                            ValidateXmlAndThrowIfInvalid(result);
                        }
                    }
                }
            }
        }
    }
}