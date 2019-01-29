using System;
using System.Collections;
using System.Linq;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class EnumerableByteExtractor : IByteExtractor
    {
        private readonly IByteExtractor _elementExtractor;

        public EnumerableByteExtractor(IByteExtractor elementExtractor)
        {
            _elementExtractor = elementExtractor;
        }

        private bool CanExtractEnumerable(Type t) => 
            typeof(IEnumerable).IsAssignableFrom(t) 
            && _elementExtractor.CanExtract(t.GenericTypeArguments.Single());

        private bool CanExtractArray(Type t) =>
            t.IsArray
            && _elementExtractor.CanExtract(t.GetElementType());

        public bool CanExtract(Type t) => CanExtractArray(t) || CanExtractEnumerable(t);

        public void Extract(object instance, System.IO.Stream stream)
        {
            foreach (var element in (IEnumerable) instance)
                _elementExtractor.Extract(element, stream);
        }
    }
}
