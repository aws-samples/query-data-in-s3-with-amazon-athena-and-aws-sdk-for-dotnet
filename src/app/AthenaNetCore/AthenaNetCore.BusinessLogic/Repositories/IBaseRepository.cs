using System;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public interface IBaseRepository : IDisposable
    {
        Task<bool> IsTheQueryStillRunning(string queryId);
    }
}