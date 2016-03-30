using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceArchive
    {
        private readonly IEnumerable<AsiceAttachableProcessor> _asiceAttachableProcessors;
        private readonly IAsiceAttachable[] _attachables;
        private ZipArchive _zipArchive;

        public AsiceArchive(IEnumerable<AsiceAttachableProcessor> asiceAttachableProcessors, params IAsiceAttachable[] attachables)
        {
            _attachables = attachables;
            _asiceAttachableProcessors = asiceAttachableProcessors;
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
                SendArchiveThroughBundleProcessors(bundleArray);

                return bundleArray;
            }
        }

        private void SendArchiveThroughBundleProcessors(byte[] archiveBytes)
        {
            foreach (var documentBundleProcessor in _asiceAttachableProcessors)
            {
                try
                {
                    documentBundleProcessor.Process(new MemoryStream(archiveBytes));
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