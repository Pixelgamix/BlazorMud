using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorMud.Contracts.Entities;

namespace BlazorMud.Contracts.Database
{
    public interface ICharacterRepository
    {
        /// <summary>
        /// Adds the specified <see cref="PlayerCharacter"/>.
        /// </summary>
        /// <param name="playerCharacter">The player character that should be added.</param>
        Task AddNewCharacterAsync(PlayerCharacter playerCharacter);
        
        /// <summary>
        /// Returns all characters that belong to the account with the specified id. 
        /// </summary>
        /// <param name="accountId">The account to return the characters for.</param>
        /// <returns>The characters belonging to the specified account.</returns>
        Task<List<PlayerCharacter>> ListCharactersByAccountIdAsync(Guid accountId);
        
    }
}