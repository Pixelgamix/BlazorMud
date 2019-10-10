using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Security;
using BlazorMud.Contracts.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazorMud.BusinessLogic.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _Logger;
        private readonly IDatabaseContext _DatabaseContext;
        private readonly IPasswordHasher _PasswordHasher;

        public AccountService(ILogger<AccountService> logger, IDatabaseContext databaseContext, IPasswordHasher passwordHasher)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<ServiceResult<Account>> LoginAsync(string username, string password)
        {
            if (username is null) throw new ArgumentNullException(nameof(username));
            if (password is null) throw new ArgumentNullException(nameof(password));

            Account account = null;

            try
            {
                await _DatabaseContext.ExecuteAsync(async u => account = await u.AccountRepository.GetAccountByNameAsync(username));

                if (account is null || !_PasswordHasher.IsSamePassword(password, account.HashedPassword))
                {
                    _Logger.LogDebug("Wrong login or password for {0}", username);
                    return new ServiceResult<Account>(false, "Wrong login or password.");
                }

                _Logger.LogDebug("{0} successfully logged in", username);
                return new ServiceResult<Account>(true, result: account);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account login for {0}", username);
                return new ServiceResult<Account>(false, "Server error. Try again later.");
            }
        }

        public async Task<ServiceResult> RegisterAsync(string username, string password)
        {
            if (username is null) throw new ArgumentNullException(nameof(username));
            if (password is null) throw new ArgumentNullException(nameof(password));

            var hashedPassword = _PasswordHasher.CreateHashedPassword(password);
            var account = new Account() { AccountName = username, HashedPassword = hashedPassword, CreatedAt = DateTime.UtcNow };

            try
            {
                var alreadyExists = false;
                await _DatabaseContext.ExecuteAsync(async u => {
                    if (await u.AccountRepository.ExistsAsync(account.AccountName))
                        alreadyExists = true;
                    else
                        await u.AccountRepository.AddNewAccountAsync(account);
                });

                if (alreadyExists)
                {
                    _Logger.LogDebug("Registration of {0} failed as the account already exits", username);
                    return new ServiceResult(false, "Account already exists.");
                }

                _Logger.LogInformation("New account registered for {0}", username);
                return new ServiceResult(true, "Account successfully created.");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account registration for {0}", username);
                return new ServiceResult(false, "Server error. Try again later.");
            }
        }

        public async Task<ServiceResult<bool>> ExistsAsync(string username)
        {
            if (username is null) throw new ArgumentNullException(nameof(username));

            try
            {
                var exists = false;
                await _DatabaseContext.ExecuteAsync(async u => exists = await u.AccountRepository.ExistsAsync(username));
                return new ServiceResult<bool>(true, result: exists);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error trying to check if username {0} exists", username);
                return new ServiceResult<bool>(false, "Server error. Try again later.");
            }
        }
    }
}
