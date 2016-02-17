using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.DataTransferObjects
{
    [TestClass]
    public class DataTransferObjectConverterTests
    {
        [TestClass]
        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsErrorSuccessfully()
            {
                //Arrange
                var source = new error
                {
                    errorcode = "errorcode",
                    errormessage = "errormessage",
                    errortype = "errortype"
                };

                var expected = new Error
                {
                    Code = source.errorcode,
                    Message = source.errormessage,
                    Type = source.errortype
                };

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                Comparator compartor = new Comparator();
                IEnumerable<IDifference> differences;
                compartor.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }    
        }
    }
}