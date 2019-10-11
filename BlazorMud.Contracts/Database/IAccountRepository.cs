using BlazorMud.Contracts.Entities;
using System.Threading.Tasks;

namespace BlazorMud.Contracts.Database
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Adds the specified <see cref="Account"/>.
        /// </summary>
        /// <param name="account">The account that should be added.</param>
        Task AddNewAccountAsync(Account account);

        /// <summary>
        /// Checks if there is an <see cref="Account"/> with the specified name registered.
        /// </summary>
        /// <param name="accountName">The account's name.</param>
        /// <returns><c>true</c> if the account exists, otherwise <c>false</c>.</returns>
        Task<bool> ExistsAsync(string accountName);

        /// <summary>
        /// Retrieves the <see cref="Account"/> with the specified account name.
        /// </summary>
        /// <param name="accountName">The account's name.</param>
        /// <returns>The account or <c>null</c>.</returns>
        Task<Account> GetAccountByNameAsync(string accountName);

        /// <summary>
        /// Updates the specified <see cref="Account"/> in the database.
        /// </summary>
        /// <param name="account">The updated account.</param>
        Task UpdateAccountAsync(Account account);
    }
}
