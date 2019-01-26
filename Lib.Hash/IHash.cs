using System;
using System.ComponentModel;

namespace Lib.Hash
{
    /// <summary>
    /// A Hash
    /// </summary>
    [TypeConverter(typeof(HashTypeConverter))]
    public interface IHash : IEquatable<IHash>
    {
        /// <summary>
        /// Bytes of the hash
        /// </summary>
        byte[] Bytes { get; }
    }
}
