using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.DomainModel;
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
        private readonly AutoMapper.IMapper _Mapper;
        private readonly ITokenGenerator _TokenGenerator;

        public AccountService(ILogger<AccountService> logger, IDatabaseContext databaseContext, IPasswordHasher passwordHasher, AutoMapper.IMapper mapper, ITokenGenerator tokenGenerator)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _TokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
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

        public async Task<ServiceResult<string>> LoginAsync(AccountLoginModel accountLogin)
        {
            if (accountLogin is null) throw new ArgumentNullException(nameof(accountLogin));

            Account account = null;
            string token = null;
            var isValidLogin = false;

            try
            {
                await _DatabaseContext.ExecuteAsync(async u =>
                {
                    account = await u.AccountRepository.GetAccountByNameAsync(accountLogin.Username);
                    isValidLogin = account != null && _PasswordHasher.IsSamePassword(accountLogin.Password, account.HashedPassword);
                    if (isValidLogin)
                    {
                        account.LastLogin = DateTime.UtcNow;
                        await u.AccountRepository.UpdateAccountAsync(account);
                        token = _TokenGenerator.Generate(account, accountLogin.ExpireMinutes);
                    }
                });

                if (!isValidLogin)
                {
                    _Logger.LogDebug("Wrong login or password for {0}", accountLogin.Username);
                    return new ServiceResult<string>(false, "Wrong login or password.");
                }

                _Logger.LogDebug("{0} successfully logged in", accountLogin.Username);
                return new ServiceResult<string>(true, result: token);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account login for {0}", accountLogin.Username);
                return new ServiceResult<string>(false, "Server error. Try again later.");
            }
        }

        public async Task<ServiceResult> RegisterAsync(AccountRegistrationModel accountRegistration)
        {
            if (accountRegistration is null) throw new ArgumentNullException(nameof(accountRegistration));

            var hashedPassword = _PasswordHasher.CreateHashedPassword(accountRegistration.Password);
            var account = _Mapper.Map<Account>(accountRegistration);

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
                    _Logger.LogDebug("Registration of {0} failed as the account already exits", accountRegistration.AccountName);
                    return new ServiceResult(false, "Account already exists.");
                }

                _Logger.LogInformation("New account registered for {0}", accountRegistration.AccountName);
                return new ServiceResult(true, "Account successfully created.");
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unexpected error during account registration for {0}", accountRegistration.AccountName);
                return new ServiceResult(false, "Server error. Try again later.");
            }
        }
    }
}
