using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Asice
{
    [TestClass]
    public class AsiceArchiveTests
    {
        [TestClass]
        public class ConstructorMethod : AsiceArchiveTests
        {
            [TestMethod]
            public void ConstructorGeneratesBytes()
            {
                //Arrange
                var asiceArchive = new AsiceArchive(new AsiceAttachableProcessor[] {}, DomainUtility.GetDirectManifest(), DomainUtility.GetSignature(), DomainUtility.GetDirectDocument());

                //Act
                var archiveBytes = asiceArchive.GetBytes();

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "manifest.xml"));
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "META-INF/signatures.xml"));
                        Assert .IsTrue(archive.Entries.Any(entry => entry.FullName == "TheFileName.pdf"));
                    }
                }
            }
        }

        [TestClass]
        public class BytesMethod : AsiceArchiveTests
        {
            [TestMethod]
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
                    Assert.IsTrue(simpleProcessor.StreamLength > 1000);
                    Assert.IsTrue(simpleProcessor.CouldReadBytesStream);
                    Assert.AreEqual(0, simpleProcessor.Initialposition);
                }
            }
        }
    }
}