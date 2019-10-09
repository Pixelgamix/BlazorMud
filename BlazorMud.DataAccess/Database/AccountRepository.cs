using BlazorMud.Contracts.Entities;
using BlazorMud.Contracts.Database;
using System;
using System.Linq;
using NHibernate;

namespace BlazorMud.DataAccess.Database
{
    public sealed class AccountRepository : IAccountRepository
    {
        public ISession Session;

        public void AddNewAccount(Account account)
        {
            Session.Save(account);
        }

        public Account GetAccountByName(string accountName)
        {
            if (accountName is null) throw new ArgumentNullException(nameof(accountName));

            return Session.Query<Account>().Where(x => x.AccountName == accountName).FirstOrDefault();
        }
    }
}
