namespace BlazorMud.Contracts.Database
{
    /// <summary>
    /// Context in which repositories can be accessed.
    /// </summary>
    public interface IRepositoryContext
    {
        /// <summary>
        /// Account repository.
        /// </summary>
        IAccountRepository AccountRepository { get; }
        
        /// <summary>
        /// Character repository.
        /// </summary>
        ICharacterRepository CharacterRepository { get; }
    }
}