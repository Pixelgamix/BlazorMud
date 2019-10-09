using System;

namespace BlazorMud.Contracts.Database
{
    public interface IDatabaseContext
    {
        void Execute(Action<IRepositoryContext> unitOfWork);
    }
}
