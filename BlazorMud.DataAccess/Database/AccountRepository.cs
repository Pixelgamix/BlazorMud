using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Entities;
using NHibernate;
using NHibernate.Linq;

namespace BlazorMud.DataAccess.Database
{
    public sealed class AccountRepository : IAccountRepository
    {
        public ISession Session;

        public Task AddNewAccountAsync(Account account)
        {
            if (account is null) throw new ArgumentNullException(nameof(account));

            return Session.SaveAsync(account);
        }

        public Task<bool> ExistsAsync(string accountName)
        {
            if (accountName is null) throw new ArgumentNullException(nameof(accountName));

            return Session.Query<Account>().Where(x => x.AccountName == accountName).AnyAsync();
        }

        public Task<Account> GetAccountByNameAsync(string accountName)
        {
            if (accountName is null) throw new ArgumentNullException(nameof(accountName));

            return Session.Query<Account>().Where(x => x.AccountName == accountName).FirstOrDefaultAsync();
        }

        public Task UpdateAccountAsync(Account account)
        {
            if (account is null) throw new ArgumentNullException(nameof(account));

            return Session.UpdateAsync(account);
        }
    }
}
