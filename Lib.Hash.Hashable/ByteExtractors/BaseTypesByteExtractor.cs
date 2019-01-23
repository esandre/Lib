using System;

namespace Lib.Hash.Hashable.ByteExtractors
{
    /// <summary>
    /// Base types byte extractor
    /// </summary>
    public class BaseTypesByteExtractor : IByteExtractor
    {
        private readonly ByteExtractorsCollection _collection;

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseTypesByteExtractor()
        {
            _collection = new ByteExtractorsCollection();

            _collection.Add(new PrimitiveByteExtractor());
            _collection.Add(new StringByteExtractor());
            _collection.Add(new DateTimeByteExtractor());
            _collection.Add(new EnumerableByteExtractor(_collection));
        }

        /// <inheritdoc />
        public bool CanExtract(Type t) => _collection.CanExtract(t);

        /// <inheritdoc />
        public void Extract(object instance, System.IO.Stream stream)
            => _collection.Extract(instance, stream);
    }
}
