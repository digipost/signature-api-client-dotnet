using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceArchive
    {
        private readonly IAsiceConfiguration _asiceConfiguration;
        private readonly IAsiceAttachable[] _attachables;
        private readonly IEnumerable<IDocumentBundleProcessor> _documentBundleProcessors = new List<IDocumentBundleProcessor>();
        private readonly bool _sendThroughBundleProcessors;
        private readonly ISignatureJob _signatureJob;

        private ZipArchive _zipArchive;

        public AsiceArchive(params IAsiceAttachable[] attachables)
        {
            _attachables = attachables;
        }

        public AsiceArchive(IEnumerable<IDocumentBundleProcessor> documentBundleProcessors, ISignatureJob signatureJob, params IAsiceAttachable[] attachables)
            : this(attachables)
        {
            _documentBundleProcessors = documentBundleProcessors;
            _signatureJob = signatureJob;
            _sendThroughBundleProcessors = _documentBundleProcessors.Any() && _signatureJob != null;
        }

        public byte[] GetBytes()
        {
            using (var stream = new MemoryStream())
            {
                _zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);

                using (_zipArchive)
                {
                    foreach (var dokument in _attachables)
                        AddToArchive(dokument.FileName, dokument.Bytes);
                }

                var bundleArray = stream.ToArray();

                if (_sendThroughBundleProcessors)
                {
                    SendArchiveThroughBundleProcessors(bundleArray);
                }

                return bundleArray;
            }
        }

        private void SendArchiveThroughBundleProcessors(byte[] archiveBytes)
        {
            foreach (var documentBundleProcessor in _documentBundleProcessors)
            {
                try
                {
                    documentBundleProcessor.Process(_signatureJob, new MemoryStream(archiveBytes));
                }
                catch (Exception exception)
                {
                    throw new IOException("Could not run stream through document bundle processor.", exception);
                }
            }
        }

        private void AddToArchive(string filename, byte[] data)
        {
            var entry = _zipArchive.CreateEntry(filename, CompressionLevel.Optimal);
            using (var stream = entry.Open())
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}