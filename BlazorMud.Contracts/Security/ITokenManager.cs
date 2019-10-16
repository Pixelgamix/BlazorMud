using BlazorMud.Contracts.Entities;

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
        /// Validates the specified token.
        /// </summary>
        /// <param name="token">The token that should be validated.</param>
        /// <returns><c>true</c> if the token is valid, otherwise <c>false</c>.</returns>
        bool Validate(string token);
    }
}
