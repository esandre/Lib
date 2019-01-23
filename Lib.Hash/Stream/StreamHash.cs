using System.Security.Cryptography;

namespace Lib.Hash.Stream
{
    /// <summary>
    /// Creates a Hash from a Stream
    /// </summary>
    public class StreamHash<THashAlgorithm> : HashAbstract, IHash<THashAlgorithm>
        where THashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StreamHash(System.IO.Stream inputStream, THashAlgorithm algorithm) 
            : base(algorithm.ComputeHash(inputStream))
        {
        }

        /// <inheritdoc />
        public bool Equals(IHash<THashAlgorithm> other) => base.Equals(other);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is IHash<THashAlgorithm> hash && Equals(hash);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(StreamHash<THashAlgorithm> left, StreamHash<THashAlgorithm> right) 
            => Equals(left, right);

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(StreamHash<THashAlgorithm> left, StreamHash<THashAlgorithm> right) 
            => !Equals(left, right);
    }
}
