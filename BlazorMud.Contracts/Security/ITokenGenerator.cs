namespace BlazorMud.Contracts.Security
{
    /// <summary>
    /// Token generator functionality.
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates a token for the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="expireMinutes">The duration in minutes for how long the token is valid.</param>
        /// <returns>The generated token.</returns>
        string Generate(string username, int expireMinutes);
    }
}
