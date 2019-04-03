using Lib.Chaining.Chain;

namespace Lib.Chaining.Structures
{
    /// <summary>
    /// A Ray is a chain with a start, but no end
    /// </summary>
    public interface IRay<out TPayload>
    {
        /// <summary>
        /// Origin of the ray
        /// </summary>
        IPredecessor<TPayload> Origin { get; }
    }
}
