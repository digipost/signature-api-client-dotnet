using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.Asice
{
    public class AsiceArchiveTests
    {
        public class ConstructorMethod : AsiceArchiveTests
        {
            [Fact]
            public void ConstructorGeneratesBytes()
            {
                //Arrange
                var document = DomainUtility.GetDirectDocument();
                var asiceArchive = new AsiceArchive(new AsiceAttachableProcessor[] { }, DomainUtility.GetDirectManifest(), DomainUtility.GetSignature(), document);

                //Act
                var archiveBytes = asiceArchive.GetBytes();

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.True(archive.Entries.Any(entry => entry.FullName == "manifest.xml"));
                        Assert.True(archive.Entries.Any(entry => entry.FullName == "META-INF/signatures.xml"));
                        Assert.True(archive.Entries.Any(entry => entry.FullName == document.FileName));
                    }
                }
            }
        }

        public class AddAttachableMethod : AsiceArchiveTests
        {
            [Fact]
            public void AddsAttachableToZip()
            {
                //Arrange
                var asiceArchive = new AsiceArchive(new AsiceAttachableProcessor[] { });

                //Act
                var attachment = DomainUtility.GetDirectDocument();
                asiceArchive.AddAttachable(attachment.FileName, attachment.Bytes);

                var archiveBytes = asiceArchive.GetBytes();

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.True(archive.Entries.Any(entry => entry.FullName == attachment.FileName));
                    }
                }
            }
        }

        public class BytesMethod : AsiceArchiveTests
        {
            [Fact]
            public void SendsBytesThroughDocumentBundleProcessor()
            {
                //Arrange
                var clientConfiguration = new ClientConfiguration(Environment.DifiQa, CoreDomainUtility.GetPostenTestCertificate())
                {
                    DocumentBundleProcessors = new List<IDocumentBundleProcessor>
                    {
                        new SimpleDocumentBundleProcessor(),
                        new SimpleDocumentBundleProcessor()
                    }
                };

                var job = DomainUtility.GetDirectJobWithSender();
                var asiceAttachableProcessors = clientConfiguration.DocumentBundleProcessors.Select(p => new AsiceAttachableProcessor(job, p));
                var asiceAttachables = new IAsiceAttachable[] {DomainUtility.GetDirectManifest(), DomainUtility.GetSignature(), DomainUtility.GetDirectDocument()};

                //Act
                var asiceArchive = new AsiceArchive(asiceAttachableProcessors, asiceAttachables);
                asiceArchive.GetBytes();

                //Assert
                foreach (var simpleProcessor in clientConfiguration.DocumentBundleProcessors.Cast<SimpleDocumentBundleProcessor>())
                {
                    Assert.True(simpleProcessor.StreamLength > 1000);
                    Assert.True(simpleProcessor.CouldReadBytesStream);
                    Assert.Equal(0, simpleProcessor.Initialposition);
                }
            }
        }
    }
}