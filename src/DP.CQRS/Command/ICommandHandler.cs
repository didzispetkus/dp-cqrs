namespace DP.CQRS
{
    /// <summary>
    /// A command handler that is capable of handling a <see cref="ICommand"/>.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle.</typeparam>
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handles the given command.
        /// </summary>
        /// <param name="command">Command to handle.</param>
        void Handle(TCommand command);
    }
}
