using System;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Random
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class RandomExtensions
    {
        public static object Next(this System.Random generator, Type desired)
        {
            switch (Type.GetTypeCode(desired))
            {
                case TypeCode.Boolean:
                    return generator.NextBool();
                case TypeCode.Byte:
                    return generator.NextByte();
                case TypeCode.Char:
                    return generator.NextChar();
                case TypeCode.DateTime:
                    return generator.NextDateTime();
                case TypeCode.Decimal:
                    return generator.NextDecimal();
                case TypeCode.Double:
                    return generator.NextDouble();
                case TypeCode.Int16:
                    return generator.NextShort();
                case TypeCode.Int32:
                    return generator.NextInt();
                case TypeCode.Int64:
                    return generator.NextLong();
                case TypeCode.SByte:
                    return generator.NextSByte();
                case TypeCode.Single:
                    return generator.NextFloat();
                case TypeCode.UInt16:
                    return generator.NextUShort();
                case TypeCode.UInt32:
                    return generator.NextUInt();
                case TypeCode.UInt64:
                    return generator.NextULong();
                default:
                    throw new ArgumentOutOfRangeException("Cannot provide a random " + desired);
            }
        }

        public static TOutput Next<TOutput>(this System.Random generator) => (TOutput) generator.Next(typeof(TOutput));

        public static bool NextBool(this System.Random generator) => generator.Next() % 2 == 0;

        public static byte NextByte(this System.Random generator) => generator.NextBytes(1)[0];

        public static sbyte NextSByte(this System.Random generator) => (sbyte) generator.NextByte();

        public static char NextChar(this System.Random generator) => BitConverter.ToChar(generator.NextBytes(2), 0);

        public static decimal NextDecimal(this System.Random generator) 
            => new decimal(
                generator.NextInt(),
                generator.NextInt(),
                generator.NextInt(),
                generator.NextBool(), 
                generator.NextByte()
            );

        public static double NextDouble(this System.Random generator) => BitConverter.ToDouble(generator.NextBytes(8), 0);

        public static float NextFloat(this System.Random generator) => BitConverter.ToSingle(generator.NextBytes(4), 0);

        public static int NextInt(this System.Random generator) => generator.Next();

        public static uint NextUInt(this System.Random generator) => BitConverter.ToUInt32(generator.NextBytes(4), 0);

        public static long NextLong(this System.Random generator) => BitConverter.ToInt64(generator.NextBytes(8), 0);

        public static ulong NextULong(this System.Random generator) => BitConverter.ToUInt64(generator.NextBytes(8), 0);

        public static short NextShort(this System.Random generator) => BitConverter.ToInt16(generator.NextBytes(2), 0);

        public static ushort NextUShort(this System.Random generator) => BitConverter.ToUInt16(generator.NextBytes(2), 0);

        public static DateTime NextDateTime(this System.Random generator)
        {
            var year = generator.Next(1900, 9999);
            var month = generator.Next(1, 12);
            var day = generator.Next(1, DateTime.DaysInMonth(year, month));
            var hour = generator.Next(0, 23);
            var minute = generator.Next(0, 59);
            var second = generator.Next(0, 59);
            return new DateTime(year, month, day, hour, minute, second);
        }

        public static byte[] NextBytes(this System.Random generator, int nb)
        {
            var buffer = new byte[nb];
            generator.NextBytes(buffer);
            return buffer;
        }
    }
}
