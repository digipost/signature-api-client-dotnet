using System.IO;
using System.IO.Compression;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal class AsiceArchive//
    {
        private readonly IAsiceAttachable[] _attachables;
        private byte[] _bytes;
        private ZipArchive _zipArchive;

        public AsiceArchive(params IAsiceAttachable[] attachables)
        {
            _attachables = attachables;
        }

        public byte[] Bytes
        {
            get { return _bytes ?? (_bytes = CreateBytes()); }
        }

        private byte[] CreateBytes()
        {
            var stream = new MemoryStream();
            _zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);

            using (_zipArchive)
            {
                foreach (var dokument in _attachables)
                    AddToArchive(dokument.FileName, dokument.Bytes);

            }

            return stream.ToArray();
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
