namespace Lib.Patterns
{
    /// <summary>
    /// A Factory
    /// </summary>
    public interface IFactory<in TInput, out TOutput>
    {
        /// <summary>
        /// Factories Output from Input
        /// </summary>
        TOutput Factory(TInput input);
    }
}
