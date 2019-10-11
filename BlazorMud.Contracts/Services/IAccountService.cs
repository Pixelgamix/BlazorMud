using BlazorMud.Contracts.DomainModel;
using BlazorMud.Contracts.Entities;
using System.Threading.Tasks;

namespace BlazorMud.Contracts.Services
{
    /// <summary>
    /// Account related services.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Fetches the <see cref="Account"/> with the specified username if the provided password is correct.
        /// </summary>
        /// <param name="accountLogin">Account information.</param>
        /// <returns>The login token if the login data is correct. Otherwise <c>null</c>.</returns>
        Task<ServiceResult<string>> LoginAsync(AccountLoginModel accountLogin);

        /// <summary>
        /// Creates an <see cref="Account" /> for the specified username with the provided password.
        /// The method only registers the account but does not perform any logging in.
        /// </summary>
        /// <param name="accountRegistration">Account information.</param>
        /// <returns>Information regarding registration success or failure.</returns>
        Task<ServiceResult> RegisterAsync(AccountRegistrationModel accountRegistration);

        /// <summary>
        /// Checks, if the specified username is registered.
        /// </summary>
        /// <param name="username">The username to check for.</param>
        /// <returns><c>true</c> if the username is registered, otherwise <c>false</c>.</returns>
        Task<ServiceResult<bool>> ExistsAsync(string username);
    }
}
