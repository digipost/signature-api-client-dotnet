using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class AccountIdTests
    {
        public class ConstructorMethod : AccountIdTests
        {
            [Fact]
            public void Sets_value()
            {
                //Arrange
                //Act
                var accountId = new AccountId("123456");

                //Assert
                Assert.Equal("123456", accountId.Value);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("   ")]
            public void Throws_on_blank_value(string value)
            {
                //Arrange
                //Act
                //Assert
                Assert.Throws<ArgumentException>(() => new AccountId(value));
            }
        }

        public class EqualsMethod : AccountIdTests
        {
            [Fact]
            public void Equal_when_value_is_equal()
            {
                //Arrange
                var first = new AccountId("123456");
                var second = new AccountId("123456");

                //Act
                //Assert
                Assert.Equal(first, second);
            }

            [Fact]
            public void Not_equal_when_value_differs()
            {
                //Arrange
                var first = new AccountId("123456");
                var second = new AccountId("654321");

                //Act
                //Assert
                Assert.NotEqual(first, second);
            }
        }
    }
}
