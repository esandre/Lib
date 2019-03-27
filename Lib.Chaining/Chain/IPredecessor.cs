namespace Lib.Chaining.Chain
{
    /// <summary>
    /// A Predecessor bears its successor's reference
    /// </summary>
    public interface IPredecessor<out TPayload> : ILink<TPayload>
    {
        /// <summary>
        /// Next link
        /// </summary>
        ISuccessor<TPayload> Next { get; }
    }
}
