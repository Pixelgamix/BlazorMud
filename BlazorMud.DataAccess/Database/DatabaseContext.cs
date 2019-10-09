using BlazorMud.Contracts.Database;
using System;

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

        public void Execute(Action<IRepositoryContext> unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            using var session = _DbContext.SessionFactory.OpenSession();
            using var transaction = session.BeginTransaction();

            _RepositoryContext.Session = session;

            try
            {
                unitOfWork(_RepositoryContext);
                session.Flush();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
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
