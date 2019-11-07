using BlazorMud.Contracts.Database;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorMud.DataAccess.Database
{
    public sealed class RepositoryContext : IRepositoryContext
    {
        public ISession Session 
        {
            get =>  _Session;
            set
            {
                if (value == _Session) return;
                _Session = value;
                _AccountRepository.Session = _Session;
                _CharacterRepository.Session = _Session;
            }
        }
        
        public IAccountRepository AccountRepository => _AccountRepository;
        public ICharacterRepository CharacterRepository => _CharacterRepository; 

        private ISession _Session;
        private AccountRepository _AccountRepository;
        private CharacterRepository _CharacterRepository;

        public RepositoryContext(AccountRepository accountRepository,
            CharacterRepository characterRepository)
        {
            _AccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _CharacterRepository = characterRepository ?? throw new ArgumentNullException(nameof(characterRepository));
        }
    }
}
