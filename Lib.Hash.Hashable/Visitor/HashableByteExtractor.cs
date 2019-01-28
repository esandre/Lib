using System;
using System.Diagnostics;
using System.IO;

namespace Lib.Hash.Hashable.Visitor
{
    /// <summary>
    /// Byte extractor for <see cref="IHashable"/> types
    /// </summary>
    public class HashableByteExtractor : IByteExtractor
    {
        private readonly IByteExtractor _elementsByteExtractor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="elementsByteExtractor"></param>
        public HashableByteExtractor(IByteExtractor elementsByteExtractor)
        {
            _elementsByteExtractor = elementsByteExtractor;
        }

        /// <inheritdoc />
        public bool CanExtract(Type t) => typeof(IHashable).IsAssignableFrom(t);

        /// <inheritdoc />
        public void Extract(object instance, Stream stream)
        {
            var hashable = instance as IHashable;
            Debug.Assert(hashable != null, nameof(hashable) + " != null");
            
            var visitor = new ByteExtractorHashableVisitor(_elementsByteExtractor, stream);
            hashable.Hash(visitor);
        }
    }
}
