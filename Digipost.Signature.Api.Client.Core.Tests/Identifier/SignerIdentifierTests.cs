using Digipost.Signature.Api.Client.Core.Identifier;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Identifier
{
    public class SignerIdentifierTests
    {
        public class IsSameAsMetod
        {
            private static readonly ContactInformation EmailAndSms = new ContactInformation {Email = new Email("email@example.com"), Sms = new Sms("33333333")};
            private static readonly ContactInformation Email = new ContactInformation {Email = new Email("email@example.com")};
            private static readonly ContactInformation Sms = new ContactInformation {Sms = new Sms("33333333")};
            private static readonly PersonalIdentificationNumber PersonalId = new PersonalIdentificationNumber("11111111111");
            private static readonly CustomIdentifier CustomIdentifier = new CustomIdentifier("123123123");

            private static readonly ContactInformation SimilarEmailAndSms = new ContactInformation {Email = new Email("email@example.com"), Sms = new Sms("33333333")};
            private static readonly ContactInformation SimilarEmail = new ContactInformation {Email = new Email("email@example.com")};
            private static readonly ContactInformation SimilarSms = new ContactInformation {Sms = new Sms("33333333")};
            private static readonly PersonalIdentificationNumber SimilarPersonalId = new PersonalIdentificationNumber("11111111111");
            private static readonly CustomIdentifier SimilarCustomIdentifier = new CustomIdentifier("123123123");

            private static readonly ContactInformation DifferentEmailAndSms = new ContactInformation {Email = new Email("differentemail@example.com"), Sms = new Sms("13333333")};
            private static readonly ContactInformation DifferentEmail = new ContactInformation {Email = new Email("differentemail@example.com")};
            private static readonly ContactInformation DifferentSms = new ContactInformation {Sms = new Sms("13333333")};
            private static readonly PersonalIdentificationNumber DifferentPersonalId = new PersonalIdentificationNumber("222222222");
            private static readonly CustomIdentifier DifferentCustomIdentifier = new CustomIdentifier("666666666");

            [Fact]
            public void DifferentImplementationsOrDataStructureAreConsideredUnequal()
            {
                Assert.False(EmailAndSms.IsSameAs(SimilarSms));
                Assert.False(EmailAndSms.IsSameAs(SimilarEmail));
                Assert.False(EmailAndSms.IsSameAs(SimilarPersonalId));
                Assert.False(EmailAndSms.IsSameAs(SimilarCustomIdentifier));
                Assert.False(CustomIdentifier.IsSameAs(SimilarEmailAndSms));
            }

            [Fact]
            public void SimilarOjectsAreConsideredEqual()
            {
                Assert.True(EmailAndSms.IsSameAs(SimilarEmailAndSms));
                Assert.True(Email.IsSameAs(SimilarEmail));
                Assert.True(Sms.IsSameAs(SimilarSms));
                Assert.True(PersonalId.IsSameAs(SimilarPersonalId));
                Assert.True(CustomIdentifier.IsSameAs(SimilarCustomIdentifier));
            }

            [Fact]
            public void SimilarStructureWithDifferentDataAreConsideredUnequal()
            {
                Assert.False(EmailAndSms.IsSameAs(DifferentEmailAndSms));
                Assert.False(Email.IsSameAs(DifferentEmail));
                Assert.False(Sms.IsSameAs(DifferentSms));
                Assert.False(PersonalId.IsSameAs(DifferentPersonalId));
                Assert.False(CustomIdentifier.IsSameAs(DifferentCustomIdentifier));
            }
        }
    }
}