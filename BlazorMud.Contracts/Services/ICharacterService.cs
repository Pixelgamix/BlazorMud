using System;
using System.Threading.Tasks;
using BlazorMud.Contracts.DomainModel;

namespace BlazorMud.Contracts.Services
{
    public interface ICharacterService
    {
        /// <summary>
        /// Fetches a list of characters that belong to the specified account.
        /// </summary>
        /// <param name="accountId">The account's id.</param>
        /// <returns>The characters belonging to the account.</returns>
        Task<ServiceResult<CharacterInfoModel[]>> ListCharacters(Guid accountId);
    }
}