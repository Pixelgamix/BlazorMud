using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Security;
using BlazorMud.Contracts.Services;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorMud.BusinessLogic.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _Logger;
        private readonly IDatabaseContext _DatabaseContext;
        private readonly IPasswordHasher _PasswordHasher;

        public AccountService(ILogger<AccountService> logger, IDatabaseContext unitOfWork, IPasswordHasher passwordHasher)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseContext = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }
        public ServiceResult<Account> Login(string username, string password)
        {
            if (username is null) throw new ArgumentNullException(nameof(username));
            if (password is null) throw new ArgumentNullException(nameof(password));

            Account account = null;

            try
            {
                _DatabaseContext.Execute(u => account = u.AccountRepository.GetAccountByName(username));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account login for {0}", username);
                return new ServiceResult<Account>(false, "Server error. Try again later.");
            }

            if (account is null || _PasswordHasher.IsSamePassword(password, account.HashedPassword))
            {
                _Logger.LogDebug("Wrong login or password for {0}", username);
                return new ServiceResult<Account>(false, "Wrong login or password.");
            }

            _Logger.LogDebug("{0} successfully logged in", username);
            return new ServiceResult<Account>(true, result: account);
        }

        public ServiceResult Register(string username, string password)
        {
            if (username is null) throw new ArgumentNullException(nameof(username));
            if (password is null) throw new ArgumentNullException(nameof(password));

            var hashedPassword = _PasswordHasher.CreateHashedPassword(password);
            var account = new Account() { AccountName = username, HashedPassword = hashedPassword, CreatedAt = DateTime.UtcNow };

            try
            {
                _DatabaseContext.Execute(u => u.AccountRepository.AddNewAccount(account));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account registration for {0}", username);
                return new ServiceResult(false, "Server error. Try again later.");
            }

            _Logger.LogInformation("New account registered for {0}", username);
            return new ServiceResult(true, "Account successfully created.");
        }
    }
}
