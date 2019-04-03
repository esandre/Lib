using System.Collections.Generic;
using Lib.Chaining.Enumeration;
using Lib.Chaining.Structures;

namespace Lib.Chaining.Extensions
{
    /// <summary>
    /// Extensions to manipulate a <see cref="IRay{TPayload}"/> as an Enumerable
    /// </summary>
    public static class RayExtensions
    {
        /// <summary>
        /// Returns an Enumerable enumerating the ray
        /// </summary>
        public static IEnumerable<TPayload> ToEnumerable<TPayload>(this IRay<TPayload> ray) => new RayEnumerableAdapter<TPayload>(ray);
    }
}
