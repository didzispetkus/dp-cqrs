namespace DP.CQRS
{
    /// <summary>
    /// Marker interface to define a generic command query responsibility segregation design pattern.
    /// <remarks>
    /// Queries returns a result and do not change the state of a system (No side effects).
    /// </remarks>
    /// </summary>
    /// <typeparam name="TResult">The return type of the query.</typeparam>
    public interface IQuery<TResult> { }
}
