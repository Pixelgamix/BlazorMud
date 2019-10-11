using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Database;
using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;

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
