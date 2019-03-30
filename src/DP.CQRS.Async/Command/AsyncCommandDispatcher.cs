using System;
using System.Threading;
using System.Threading.Tasks;

namespace DP.CQRS.Async
{
    public class AsyncCommandDispatcher : IAsyncCommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public AsyncCommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(IAsyncCommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException(handlerType);
            }

            await handler.HandleAsync((dynamic)command, cancellationToken);
        }
    }
}
