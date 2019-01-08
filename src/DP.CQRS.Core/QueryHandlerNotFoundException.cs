using System;

namespace DP.CQRS
{
    /// <summary>
    /// The exception that is thrown when query handler cannot be found.
    /// </summary>
    public class QueryHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandlerNotFoundException"/> class tith a specified handler type.
        /// </summary>
        /// <param name="handlerType">The handler type that cannot be found.</param>
        public QueryHandlerNotFoundException(Type handlerType)
            : base($"Handler was not found for query of type {handlerType.FullName}. Make sure handler is registered with your container.")
        {
            HandlerType = handlerType;
        }

        /// <summary>
        /// Get the type of the query handler that causes this exception.
        /// </summary>
        public Type HandlerType { get; }
    }
}
