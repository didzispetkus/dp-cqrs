namespace DP.CQRS
{
    /// <summary>
    /// Represents a dispatcher used to dispatch <see cref="ICommand"/> instance to its handler.
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatches the given command to its handler.
        /// </summary>
        /// <typeparam name="TCommand">The type of command to handle.</typeparam>
        /// <param name="command">Command to handle.</param>
        void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
