using System;
using System.Linq;

namespace Lib.Api
{
    /// <summary>
    /// <see cref="Uri"/> extensions
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Appends chunks to an Uri, separated by /
        /// </summary>
        public static Uri Append(this Uri uri, params string[] paths)
        {
            return new Uri(
                paths.Aggregate(
                    uri.AbsoluteUri, 
                    (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
        }
    }
}
