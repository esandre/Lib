using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class ByteExtractorsCollection : IByteExtractor
    {
        private readonly ICollection<IByteExtractor> _collection;

        public ByteExtractorsCollection()
        {
            _collection = new List<IByteExtractor>();
        }

        public void Add(IByteExtractor extractor) => _collection.Add(extractor);
        
        public bool CanExtract(Type t) => _collection.Any(element => element.CanExtract(t));

        public void Extract(object instance, System.IO.Stream stream)
        {
            try
            {
                _collection.First(element => element.CanExtract(instance.GetType())).Extract(instance, stream);
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException($"No extractor compatible with type {instance.GetType()}", e);
            }
        }
    }
}
