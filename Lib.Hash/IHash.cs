using System;
using System.ComponentModel;
using System.Security.Cryptography;

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

    /// <summary>
    /// An algorithm-specific type of Hash
    /// </summary>
    public interface IHash<THashAlgorithm> : IHash, IEquatable<IHash<THashAlgorithm>> where THashAlgorithm : HashAlgorithm
    {
    }
}
