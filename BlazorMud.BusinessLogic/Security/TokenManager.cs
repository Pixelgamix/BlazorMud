using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorMud.BusinessLogic.Security
{
    public sealed class TokenManager : ITokenManager
    {
        private readonly SecuritySettings _SecuritySettings;

        public TokenManager(SecuritySettings securitySettings)
        {
            this._SecuritySettings = securitySettings ?? throw new ArgumentNullException(nameof(securitySettings));
        }

        public string Generate(Account account, int expireMinutes)
        {
            // Implementation based on https://stackoverflow.com/a/40284152

            if (account is null) throw new ArgumentNullException(nameof(account));

            var symmetricKey = Convert.FromBase64String(_SecuritySettings.Tokens.Key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _SecuritySettings.Tokens.Audience,
                Issuer = _SecuritySettings.Tokens.Issuer,
                IssuedAt = now,
                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.AccountName),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString("N"))
                })
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public bool Validate(string token)
        {
            var symmetricKey = Convert.FromBase64String(_SecuritySettings.Tokens.Key);

            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                ValidAudience = _SecuritySettings.Tokens.Audience,
                ValidIssuer = _SecuritySettings.Tokens.Issuer,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }

            return validatedToken != null;
        }
    }
}
