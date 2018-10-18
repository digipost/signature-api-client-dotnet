using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice
{
    internal class AsiceArchive
    {
        private readonly IEnumerable<AsiceAttachableProcessor> _asiceAttachableProcessors;
        private readonly List<KeyValuePair<string, byte[]>> _attachables = new List<KeyValuePair<string, byte[]>>();
        private ZipArchive _zipArchive;

        public AsiceArchive(IEnumerable<AsiceAttachableProcessor> asiceAttachableProcessors, params IAsiceAttachable[] attachables)
        {
            _attachables.AddRange(attachables.Select(a => new KeyValuePair<string, byte[]>(a.FileName, a.Bytes)));
            _asiceAttachableProcessors = asiceAttachableProcessors;
        }

        public void AddAttachable(string FileName, byte[] attachable)
        {
            _attachables.Add(new KeyValuePair<string, byte[]>(FileName, attachable));
        }

        public byte[] GetBytes()
        {
            using (var stream = new MemoryStream())
            {
                _zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);

                using (_zipArchive)
                {
                    foreach (var attachable in _attachables)
                    {
                        AddToArchive(attachable.Key, attachable.Value);
                    }
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
