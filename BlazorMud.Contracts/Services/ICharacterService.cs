using System;
using System.Threading.Tasks;
using BlazorMud.Contracts.DomainModel;

namespace BlazorMud.Contracts.Services
{
    public interface ICharacterService
    {
        /// <summary>
        /// Creates a new character.
        /// </summary>
        /// <param name="characterCreationModel">The character to create.</param>
        /// <returns>Info on success/failure of character creation.</returns>
        Task<ServiceResult> CreateCharacterAsync(CharacterCreationModel characterCreationModel);

        /// <summary>
        /// Fetches the character with the specified id.
        /// </summary>
        /// <param name="characterId">The character's id.</param>
        /// <returns>The character.</returns>
        Task<ServiceResult<CharacterInfoModel>> FetchCharacterById(Guid characterId);
        
        /// <summary>
        /// Fetches a list of characters that belong to the specified account.
        /// </summary>
        /// <param name="accountId">The account's id.</param>
        /// <returns>The characters belonging to the account.</returns>
        Task<ServiceResult<CharacterInfoModel[]>> ListCharactersAsync(Guid accountId);
    }
}