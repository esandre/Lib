using System;
using System.IO;
using System.Security.Cryptography;
using Lib.Hash.Hashable.ByteExtractors;

namespace Lib.Hash.Hashable
{
    /// <summary>
    /// Automatic hash factory
    /// </summary>
    public class HashFactory<THashAlgorithm> : IHashFactory
        where THashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// ErrorMessage triggered when an instance cannot be hashed
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        public static readonly Func<object, string> CannotHashMessage = obj => $"Cannot hash object {obj} of type {obj.GetType()}";
        
        private readonly Func<THashAlgorithm> _algorithmGenerator;
        private readonly IByteExtractor _byteExtractor;

        public HashFactory(Func<THashAlgorithm> algorithmGenerator, IByteExtractor byteExtractor)
        {
            _algorithmGenerator = algorithmGenerator;
            _byteExtractor = byteExtractor;
        }

        public HashFactory(Func<THashAlgorithm> algorithmGenerator, params IByteExtractor[] byteExtractor)
        {
            _algorithmGenerator = algorithmGenerator;
            var extractors = new ByteExtractorsCollection();
            foreach (var extractor in byteExtractor) extractors.Add(extractor);
            _byteExtractor = extractors;
        }

        /// <inheritdoc />
        public IHash Factory(object input)
        {
            if(!_byteExtractor.CanExtract(input.GetType()))
                throw new ArgumentException(CannotHashMessage(input));

            using (var stream = new MemoryStream())
            using (var safeStream = Stream.Synchronized(stream))
            {
                _byteExtractor.Extract(input, safeStream);
                safeStream.Position = 0;
                return new StreamHash(safeStream, _algorithmGenerator.Invoke());
            }
        }
    }
}
