using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Entities;
using NHibernate;
using NHibernate.Linq;

namespace BlazorMud.DataAccess.Database
{
    public class CharacterRepository : ICharacterRepository
    {
        public ISession Session;
        
        public Task AddNewCharacterAsync(PlayerCharacter playerCharacter)
        {
            if (playerCharacter == null) throw new ArgumentNullException(nameof(playerCharacter));

            return Session.SaveAsync(playerCharacter);
        }

        public Task<List<PlayerCharacter>> ListCharactersByAccountIdAsync(Guid accountId)
        {
            return Session.Query<PlayerCharacter>().Where(x => x.Account.AccountId == accountId).ToListAsync();
        }
    }
}