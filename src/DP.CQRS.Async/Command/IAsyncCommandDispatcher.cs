using System.Threading;
using System.Threading.Tasks;

namespace DP.CQRS.Async
{
    public interface IAsyncCommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default);
    }
}
