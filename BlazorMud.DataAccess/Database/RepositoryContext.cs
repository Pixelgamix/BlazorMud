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
                if (value != _Session)
                {
                    _Session = value;
                    _AccountRepository.Session = _Session;
                }
            }
        }
        
        public IAccountRepository AccountRepository => _AccountRepository;

        private ISession _Session;
        private AccountRepository _AccountRepository;

        public RepositoryContext(AccountRepository accountRepository)
        {
            _AccountRepository = accountRepository;
        }
    }
}
