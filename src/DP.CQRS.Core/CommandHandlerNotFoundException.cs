using System;

namespace DP.CQRS
{
    /// <summary>
    /// The exception that is thrown when command handler cannot be found.
    /// </summary>
    public class CommandHandlerNotFoundException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerNotFoundException"/> class with a specified handler type.
        /// </summary>
        /// <param name="handlerType">The handler type that cannot be found.</param>
        public CommandHandlerNotFoundException(Type handlerType)
            : base($"Handler was not found for command of type {handlerType.FullName}. Make sure handler is registered with your container.")
        {
            HandlerType = handlerType;
        }

        /// <summary>
        /// Get the type of the command handler that causes this exception.
        /// </summary>
        public Type HandlerType { get; }
    }
}
