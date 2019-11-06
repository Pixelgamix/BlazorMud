using System;
using BlazorMud.Contracts.Entities;
using BlazorMud.DataAccess.Database;
using BlazorMud.DataAccess.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Xunit;

namespace BlazorMud.DataAccess.Test
{
    public class AccountRepositoryTest : IDisposable
    {
        private readonly ISessionFactory _SessionFactory;
        
        public AccountRepositoryTest()
        {
            _SessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("AccountRepositoryTest.sqlite"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AccountMapping>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(true, true, false))
                .BuildSessionFactory();
        }
        
        [Fact]
        public async void AccountRepositoryIntegration()
        {
            using var session = _SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();
            
            var accountRepository = new AccountRepository { Session = session };

            var account = new Account()
            {
                AccountName = "testaccount",
                HashedPassword = "123456789012345678901234567890123456789012345678"
            };

            // Create account
            await accountRepository.AddNewAccountAsync(account);
            
            // Check if account exists
            var exists = await accountRepository.ExistsAsync("testaccount");
            Assert.True(exists);
            
            // Fetch account by name
            account = await accountRepository.GetAccountByNameAsync("testaccount");
            Assert.NotNull(account);
            Assert.Equal("testaccount", account.AccountName);

            // Change password
            account.HashedPassword = "999999999999999999999999999999999999999999999999";
            await accountRepository.UpdateAccountAsync(account);

            // Check if account can be fetched by id
            account = await accountRepository.GetAccountByIdAsync(account.AccountId);
            Assert.NotNull(account);
            Assert.Equal("testaccount", account.AccountName);
            Assert.Equal("999999999999999999999999999999999999999999999999", account.HashedPassword);
           
            await session.FlushAsync();
            await transaction.CommitAsync();
        }

        public void Dispose()
        {
            _SessionFactory.Dispose();
        }
    }
}