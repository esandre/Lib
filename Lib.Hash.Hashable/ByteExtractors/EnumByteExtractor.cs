using System;
using System.IO;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class EnumByteExtractor : IByteExtractor
    {
        private readonly PrimitiveByteExtractor _primitiveByteExtractor;

        public EnumByteExtractor(PrimitiveByteExtractor primitiveByteExtractor)
        {
            _primitiveByteExtractor = primitiveByteExtractor;
        }

        public bool CanExtract(Type t) => t.IsEnum;

        public void Extract(object instance, Stream stream)
        {
            var enumType = Enum.GetUnderlyingType(instance.GetType());
            _primitiveByteExtractor.Extract(Convert.ChangeType(instance, enumType), stream);
        }
    }
}
