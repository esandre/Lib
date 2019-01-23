using System;
using System.Linq;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class PrimitiveByteExtractor : IByteExtractor
    {
        public bool CanExtract(Type t) => t.IsPrimitive || t == typeof(decimal);

        public void Extract(object instance, System.IO.Stream stream)
        {
            var bytes = GetBytes(instance);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static byte[] GetBytes(object instance)
        {
            switch (Type.GetTypeCode(instance.GetType()))
            {
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool)instance);
                case TypeCode.Byte:
                    return new []{ (byte) instance };
                case TypeCode.Char:
                    return BitConverter.GetBytes((char)instance);
                case TypeCode.Decimal:
                    return decimal.GetBits((decimal) instance)
                        .SelectMany(BitConverter.GetBytes)
                        .ToArray();
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)instance);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)instance);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)instance);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)instance);
                case TypeCode.SByte:
                    return BitConverter.GetBytes((sbyte)instance);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)instance);
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)instance);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)instance);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)instance);
                default:
                    throw new ArgumentOutOfRangeException("Not a primitive : " + instance.GetType());
            }
        }
    }
}
