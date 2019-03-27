namespace Lib.Chaining.Chain
{
    /// <summary>
    /// A Successor bears its predecessor's reference
    /// </summary>
    public interface ISuccessor<out TPayload> : ILink<TPayload>
    {
        /// <summary>
        /// Previous link
        /// </summary>
        IPredecessor<TPayload> Previous { get; }
    }
}
