using Lib.Chaining.Chain;

namespace Lib.Chaining.Structures
{
    /// <summary>
    /// A Segment is a chain with a Start and an End
    /// </summary>
    public interface ISegment<out TPayload>
    {
        /// <summary>
        /// Segment start
        /// </summary>
        IPredecessor<TPayload> Start { get; }

        /// <summary>
        /// Segment end
        /// </summary>
        ISuccessor<TPayload> End { get; }
    }
}
