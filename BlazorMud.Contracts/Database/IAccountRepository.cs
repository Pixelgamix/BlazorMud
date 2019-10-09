using BlazorMud.Contracts.Entities;

namespace BlazorMud.Contracts.Database
{
    public interface IAccountRepository
    {
        void AddNewAccount(Account account);
        Account GetAccountByName(string accountName);
    }
}
