using System.Security.Cryptography;

namespace Lib.Hash
{
    /// <summary>
    /// Creates a Hash from a Stream
    /// </summary>
    public class StreamHash : HashAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StreamHash(System.IO.Stream inputStream, HashAlgorithm algorithm) 
            : base(algorithm.ComputeHash(inputStream))
        {
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is IHash hash && Equals(hash);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(StreamHash left, StreamHash right) 
            => Equals(left, right);

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(StreamHash left, StreamHash right) 
            => !Equals(left, right);
    }
}
