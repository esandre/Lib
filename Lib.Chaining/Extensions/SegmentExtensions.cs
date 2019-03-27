using Lib.Chaining.Enumeration;
using Lib.Chaining.Structures;

namespace Lib.Chaining.Extensions
{
    /// <summary>
    /// Extensions to manipulate a <see cref="ISegment{TPayload}"/> as a Collection
    /// </summary>
    public static class SegmentExtensions
    {
        /// <summary>
        /// Returns a collection enumerating the segment
        /// </summary>
        public static IReadonlyReversibleCollection<TPayload> ToReadonlyReversibleCollection<TPayload>(this ISegment<TPayload> segment)
            => new SegmentReadonlyCollectionAdapter<TPayload>(segment);
    }
}
