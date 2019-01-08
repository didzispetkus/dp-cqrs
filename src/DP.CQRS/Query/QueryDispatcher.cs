using System;

namespace DP.CQRS
{
    /// <inheritdoc/>
    public class QueryDispatcher : IQueryDispatcher
    {

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="QueryDispatcher"/> class with specified <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="serviceProvider"/> is <see langword="null"/>.
        /// </exception>
        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Dispatches and returns result specified by the given query.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by query.</typeparam>
        /// <param name="query">Query to handle.</param>
        /// <returns>The result specified by query.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="query"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="QueryHandlerNotFoundException">
        /// Thrown when handler can not be resolved using <see cref="IServiceProvider"/>.
        /// </exception>
        TResult IQueryDispatcher.Dispatch<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new QueryHandlerNotFoundException(handlerType);
            }

            return handler.Handle((dynamic)query);
        }
    }
}
