namespace BlazorMud.Contracts.Database
{
    public interface IRepositoryContext
    {
        IAccountRepository AccountRepository { get; }
    }
}