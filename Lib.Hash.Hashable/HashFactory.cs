using System;
using System.IO;
using System.Security.Cryptography;
using Lib.Hash.Stream;
using Lib.Patterns;

namespace Lib.Hash.Hashable
{
    /// <summary>
    /// Automatic hash factory
    /// </summary>
    public class HashFactory<THashAlgorithm> : IFactory<object, IHash<THashAlgorithm>> 
        where THashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// ErrorMessage triggered when an instance cannot be hashed
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        public static readonly Func<object, string> CannotHashMessage = obj => $"Cannot hash object {obj} of type {obj.GetType()}";

        private readonly THashAlgorithm _algorithm;
        private readonly IByteExtractor _byteExtractor;

        /// <inheritdoc />
        public HashFactory(THashAlgorithm algorithm, IByteExtractor byteExtractor)
        {
            _algorithm = algorithm;
            _byteExtractor = byteExtractor;
        }

        /// <inheritdoc />
        public IHash<THashAlgorithm> Factory(object input)
        {
            if(!_byteExtractor.CanExtract(input.GetType()))
                throw new ArgumentException(CannotHashMessage(input));

            using (var stream = new MemoryStream())
            {
                _byteExtractor.Extract(input, stream);
                stream.Position = 0;
                return new StreamHash<THashAlgorithm>(stream, _algorithm);
            }
        }
    }
}
