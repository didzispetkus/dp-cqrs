using System.Threading;
using System.Threading.Tasks;

namespace DP.CQRS.Async
{
    public interface IAsyncCommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
