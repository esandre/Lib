using System;
using System.Linq;

namespace Lib.Reflection
{
    /// <summary>
    /// <see cref="decimal"/> extensions
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Returns decimal number bytes
        /// </summary>
        public static byte[] GetBytes(this decimal dec) 
            => decimal.GetBits(dec).SelectMany(BitConverter.GetBytes).ToArray();
    }
}
