﻿using System.Collections.Generic;
using System.Net;
using Digipost.Signature.Api.Client.Core.Internal.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Schemas;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.DataTransferObjects
{
    public class DataTransferObjectConverterTests
    {
        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_error_successfully()
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
                var compartor = new Comparator();
                IEnumerable<IDifference> differences;
                compartor.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }
        }
    }
}
