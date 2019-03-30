using System;
using System.Threading;
using System.Threading.Tasks;

namespace DP.CQRS.Async
{
    public class AsyncQueryDispatcher : IAsyncQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public AsyncQueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }


        public Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var handlerType = typeof(IAsyncQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new QueryHandlerNotFoundException(handlerType);
            }

            return handler.HandleAsync((dynamic)query, cancellationToken);
        }
    }
}
