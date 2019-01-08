using System;

namespace DP.CQRS
{
    /// <inheritdoc/>
    public class CommandDispatcher : ICommandDispatcher
    {

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandDispatcher"/> class with specified <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="serviceProvider"/> is <see langword="null"/>.
        /// </exception>
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// Dispatches the given command to its handler.
        /// </summary>
        /// <typeparam name="TCommand">The type of command to handle.</typeparam>
        /// <param name="command">Command to handle.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="command"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="CommandHandlerNotFoundException">
        /// Thrown when handler can not be resolved using <see cref="IServiceProvider"/>.
        /// </exception>
        void ICommandDispatcher.Dispatch<TCommand>(TCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = _serviceProvider.GetService(handlerType);
            if (handler == null)
            {
                throw new CommandHandlerNotFoundException(handlerType);
            }

            handler.Handle((dynamic)command);
        }
    }
}
