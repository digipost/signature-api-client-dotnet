using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice
{
    [TestClass]
    public class AsiceArchiveTests
    {
        [TestClass]
        public class ConstructorMethod : AsiceArchiveTests
        {
            [TestMethod]
            public void SimpleConstructorGeneratesBytes()
            {
                //Arrange
                var asiceArchive = new AsiceArchive(DomainUtility.GetManifest(), DomainUtility.GetSignature(), DomainUtility.GetDocument());

                //Act
                var archiveBytes = asiceArchive.Bytes;

                //Assert
                using (var memoryStream = new MemoryStream(archiveBytes))
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "manifest.xml"));
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "META-INF/signatures.xml"));
                        Assert.IsTrue(archive.Entries.Any(entry => entry.FullName == "TestFileName.pdf"));
                    }
                }
            }
        }
    }
}