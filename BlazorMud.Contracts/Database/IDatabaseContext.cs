using System;
using System.Threading.Tasks;

namespace BlazorMud.Contracts.Database
{
    public interface IDatabaseContext
    {
        Task ExecuteAsync(Func<IRepositoryContext, Task> unitOfWork);
    }
}
