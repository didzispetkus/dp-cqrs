namespace DP.CQRS
{
    /// <summary>
    /// A query handler that is capable of handling a <see cref="IQuery{TResult}"/>.
    /// </summary>
    /// <typeparam name="TQuery">The type of query to handle.</typeparam>
    /// <typeparam name="TResult">The type of result returned by query.</typeparam>
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

        /// <summary>
        /// Returns result specified by the given query.
        /// </summary>
        /// <param name="query">Query to handle.</param>
        /// <returns>The result specified by query.</returns>
        TResult Handle(TQuery query);
    }
}
