using BlazorMud.Contracts.Entities;

namespace BlazorMud.Contracts.Services
{
    public interface IAccountService
    {
        ServiceResult<Account> Login(string username, string password);

        ServiceResult Register(string username, string password);
    }
}
