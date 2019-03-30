using System.Threading;
using System.Threading.Tasks;

namespace DP.CQRS.Async
{
    public interface IAsyncQueryDispatcher
    {
        Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}
