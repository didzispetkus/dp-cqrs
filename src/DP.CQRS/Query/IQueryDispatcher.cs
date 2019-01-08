namespace DP.CQRS
{
    /// <summary>
    /// Represents a dispatcher used to dispatch <see cref="IQuery{TResult}"/> instance to its handler.
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Dispatches and returns result specified by the given query.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by query.</typeparam>
        /// <param name="query">Query to handle.</param>
        /// <returns>The result specified by query.</returns>
        TResult Dispatch<TResult>(IQuery<TResult> query);
    }
}
