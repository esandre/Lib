using System.Collections.Generic;

namespace Lib.Hash
{
    /// <summary>
    /// An abstract Hash
    /// </summary>
    public abstract class HashAbstract : IHash
    {
        private static readonly IEqualityComparer<IHash> EqualityComparer = new HashEqualityComparer();

        /// <summary>
        /// Constructor
        /// </summary>
        protected HashAbstract(byte[] bytes)
        {
            Bytes = bytes;
        }

        /// <inheritdoc />
        public byte[] Bytes { get; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((HashAbstract)obj);
        }

        /// <inheritdoc />
        public bool Equals(IHash other)
            => EqualityComparer.Equals(this, other);

        /// <inheritdoc />
        public override int GetHashCode()
            => EqualityComparer.GetHashCode(this);

        /// <inheritdoc />
        public override string ToString()
            => HashStringRepresentation.Process(this);
    }
}
