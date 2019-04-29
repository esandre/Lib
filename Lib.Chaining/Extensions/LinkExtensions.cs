using Lib.Chaining.Chain;
using Lib.Chaining.Enumeration.Reversible;

namespace Lib.Chaining.Extensions
{
    /// <summary>
    /// Extensions to enumerate <see cref="ILink{TPayload}"/>
    /// </summary>
    public static class LinkExtensions
    {
        /// <summary>
        /// Creates a cursor
        /// </summary>
        /// <typeparam name="TPayload"></typeparam>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static IReversibleEnumerable<TPayload> ToReversibleEnumerable<TPayload>(this ILink<TPayload> origin)
            => new LinkReversibleEnumerable<TPayload>(origin);
    }
}
