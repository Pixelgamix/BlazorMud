using BlazorMud.Contracts.Database;
using System;
using System.Threading.Tasks;

namespace BlazorMud.DataAccess.Database
{
    public sealed class DatabaseContext : IDatabaseContext
    {
        private DbContext _DbContext;
        private RepositoryContext _RepositoryContext;

        public DatabaseContext(DbContext dbContext,
            RepositoryContext repositoryContext)
        {

            _DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _RepositoryContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public async Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            using var session = _DbContext.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            _RepositoryContext.Session = session;

            try
            {
                await unitOfWork(_RepositoryContext);
                await session.FlushAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                _RepositoryContext.Session = null;
                session.Dispose();
            }
            
        }
    }
}
