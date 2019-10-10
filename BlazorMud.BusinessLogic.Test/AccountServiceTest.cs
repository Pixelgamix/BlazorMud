using BlazorMud.BusinessLogic.Services;
using BlazorMud.Contracts.Database;
using BlazorMud.Contracts.Security;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BlazorMud.BusinessLogic.Test
{
    public sealed class AccountServiceTest
    {
        [Fact]
        public async void ExistsAsync_Account_Exists()
        {
            var loggerMock = new Mock<ILogger<AccountService>>();

            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(acc => acc.ExistsAsync("testuser"))
                .Returns(Task.FromResult(true));

            var repositoryContextMock = new Mock<IRepositoryContext>();
            repositoryContextMock.SetupGet(repo => repo.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var databaseContextMock = new Mock<IDatabaseContext>();
            databaseContextMock.Setup(context => context.ExecuteAsync(It.IsAny<Func<IRepositoryContext, Task>>()))
                .Callback(async (Func<IRepositoryContext, Task> f) => await f(repositoryContextMock.Object));

            var passwordHasherMock = new Mock<IPasswordHasher>();

            var subject = new AccountService(loggerMock.Object, databaseContextMock.Object, passwordHasherMock.Object);

            var result = await subject.ExistsAsync("testuser");

            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Message);
            Assert.True(result.Result);
        }

        [Fact]
        public async void ExistsAsync_Account_Not_Exists()
        {
            var loggerMock = new Mock<ILogger<AccountService>>();

            var accountRepositoryMock = new Mock<IAccountRepository>();
            accountRepositoryMock.Setup(acc => acc.ExistsAsync("testuser"))
                .Returns(Task.FromResult(false));

            var repositoryContextMock = new Mock<IRepositoryContext>();
            repositoryContextMock.SetupGet(repo => repo.AccountRepository)
                .Returns(accountRepositoryMock.Object);

            var databaseContextMock = new Mock<IDatabaseContext>();
            databaseContextMock.Setup(context => context.ExecuteAsync(It.IsAny<Func<IRepositoryContext, Task>>()))
                .Callback(async (Func<IRepositoryContext, Task> f) => await f(repositoryContextMock.Object));

            var passwordHasherMock = new Mock<IPasswordHasher>();

            var subject = new AccountService(loggerMock.Object, databaseContextMock.Object, passwordHasherMock.Object);

            var result = await subject.ExistsAsync("testuser");

            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Message);
            Assert.False(result.Result);
        }

        [Fact]
        public async void ExistsAsync_Account_Exception_Caught()
        {
            var loggerMock = new Mock<ILogger<AccountService>>();

            var databaseContextMock = new Mock<IDatabaseContext>();
            databaseContextMock.Setup(context => context.ExecuteAsync(It.IsAny<Func<IRepositoryContext, Task>>()))
                .Throws(new InvalidOperationException());

            var passwordHasherMock = new Mock<IPasswordHasher>();

            var subject = new AccountService(loggerMock.Object, databaseContextMock.Object, passwordHasherMock.Object);

            var result = await subject.ExistsAsync("testuser");

            Assert.False(result.IsSuccess);
            Assert.Equal("Server error. Try again later.", result.Message);
            Assert.False(result.Result);
        }
    }
}
