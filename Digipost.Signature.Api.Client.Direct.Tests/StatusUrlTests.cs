using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class StatusUrlTests
    {
        public class ConstructorMethod : StatusUrlTests
        {
            [Fact]
            public void Can_be_initialized_with_null_string_throws_on_status_url()
            {
                var actual = new StatusUrl(null);

                Assert.Throws<InvalidOperationException>(() => actual.StatusBaseUrl);
            }
        }

        public class StatusMethod : StatusUrlTests
        {
            [Fact]
            public void Returns_status_not_null()
            {
                const string statusUrl = "http://statusurl.no";
                const string statusQueryToken = "StatusQueryToken";

                var actual = new StatusUrl(new Uri(statusUrl));

                Assert.NotNull(actual.Status(statusQueryToken));
            }
        }
    }
}
