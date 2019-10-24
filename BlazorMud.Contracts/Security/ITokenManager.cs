using System.Security.Claims;
using BlazorMud.Contracts.Entities;
using Microsoft.IdentityModel.Tokens;

namespace BlazorMud.Contracts.Security
{
    /// <summary>
    /// Token generator functionality.
    /// </summary>
    public interface ITokenManager
    {
        /// <summary>
        /// Generates a token for the specified username.
        /// </summary>
        /// <param name="account">The <see cref="Account"/> to generate the token for.</param>
        /// <param name="expireMinutes">The duration in minutes for how long the token is valid.</param>
        /// <returns>The generated token.</returns>
        string Generate(Account account, int expireMinutes);

        /// <summary>
        /// Validates the specified token and returns a principal with the token's data.
        /// </summary>
        /// <param name="token">The token that should be validated.</param>
        /// <param name="validatedToken">The validated token or <c>null</c> if validation failed.</param>
        /// <returns>The principal or <c>null</c>.</returns>
        ClaimsPrincipal Validate(string token, out SecurityToken validatedToken);
    }
}
