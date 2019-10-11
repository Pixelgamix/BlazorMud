using BlazorMud.BusinessLogic.Mappings;
using BlazorMud.Contracts.Security;
using Moq;
using Xunit;

namespace BlazorMud.BusinessLogic.Test
{
    public sealed class AccountMappingProfileTest
    {
        [Fact]
        public void AccountMappingProfile_Is_Valid()
        {
            var passwordHasherMock = new Mock<IPasswordHasher>();

            var subject = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountMappingProfile(passwordHasherMock.Object));
            });

            subject.AssertConfigurationIsValid();
        }
    }
}
