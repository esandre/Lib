using Lib.Chaining.Enumeration.Reversible;

namespace Lib.Chaining.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IReversibleEnumerable{TElement}"/>
    /// </summary>
    public static class ReversibleEnumerableExtensions
    {
        /// <summary>
        /// Narrows an enumerable to a segment. Boundaries are included.
        /// </summary>
        public static IReadonlyReversibleCollection<TPayload> Narrow<TPayload>(
            this IReversibleEnumerable<TPayload> enumerable, 
            TPayload lowerBoundary, 
            TPayload higherBoundary) =>
            new NarrowedReadonlyReversibleCollectionAdapter<TPayload>(
                enumerable, 
                lowerBoundary,
                higherBoundary);
    }
}
