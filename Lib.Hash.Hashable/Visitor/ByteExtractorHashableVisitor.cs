using System;
using System.IO;

namespace Lib.Hash.Hashable.Visitor
{
    internal class ByteExtractorHashableVisitor : IHashableVisitor
    {
        private readonly IByteExtractor _byteExtractor;
        private readonly Stream _stream;

        public ByteExtractorHashableVisitor(IByteExtractor byteExtractor, Stream stream)
        {
            _byteExtractor = byteExtractor;
            _stream = stream;
        }

        public void HashData(object data)
        {
            if(!_byteExtractor.CanExtract(data.GetType()))
                throw new InvalidOperationException("Cannot hash " + data.GetType());

            _byteExtractor.Extract(data, _stream);
        }

        public void HashOther(IHashable other)
        {
            other.Hash(this);
        }
    }
}
