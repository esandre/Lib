using Lib.Chaining.Structures;

namespace Lib.Chaining.Enumeration
{
    /// <summary>
    /// Enumerates a Ray as a suit of Links
    /// </summary>
    internal class RayEnumerableAdapter<TPayload> : PredecessorEnumerable<TPayload>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RayEnumerableAdapter(IRay<TPayload> ray) : base(ray.Origin)
        {
        }
    }
}
