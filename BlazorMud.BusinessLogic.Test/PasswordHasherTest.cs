using BlazorMud.BusinessLogic.Security;
using Xunit;

namespace BlazorMud.BusinessLogic.Test
{
    public sealed class PasswordHasherTest
    {
        [Fact]
        public void CreateHashedPassword_IsSamePassword()
        {
            var hasher = new PasswordHasher();
            var password = "TWz6_Zj3C#$+AxK[";

            var hashedPassword = hasher.CreateHashedPassword(password);
            Assert.True(hasher.IsSamePassword(password, hashedPassword));
        }

        [Theory]
        [InlineData("fZbU+]9bT${ve8{")]
        [InlineData("M2=BTr(5cDU!] r")]
        [InlineData("N6%KE]z2-&Z{J")]
        [InlineData("RyZ5$6rm$J)A")]
        [InlineData("[_>NaX`N8 /")]
        public void CreateHashedPassword_Length_Always_48(string password)
        {
            var hasher = new PasswordHasher();
            var hashedPassword = hasher.CreateHashedPassword(password);
            Assert.Equal(48, hashedPassword.Length);
        }
    }
}
