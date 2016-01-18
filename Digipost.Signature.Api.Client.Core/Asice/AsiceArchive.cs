using System;
using System.IO;
using System.IO.Compression;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;

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

        public string Filnavn
        {
            get { return "post.asice.zip"; }
        }

        public byte[] Bytes
        {
            get
            {
                if (_bytes != null)
                    return _bytes;

                return _bytes = CreateBytes();
            }
        }

        public string Innholdstype
        {
            get { return "application/cms"; }
        }

        public string ContentId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string TransferEncoding
        {
            get { return "binary"; }
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
