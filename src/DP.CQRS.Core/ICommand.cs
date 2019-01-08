namespace DP.CQRS
{
    /// <summary>
    /// Marker interface to define a command from command query responsibility segregation design pattern.
    /// <remarks>
    /// Commands changes the state of a system but do not return a value.
    /// </remarks>
    /// </summary>
    public interface ICommand { }
}
