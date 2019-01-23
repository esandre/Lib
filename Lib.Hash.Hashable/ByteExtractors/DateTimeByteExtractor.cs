using System;
using System.Diagnostics;

namespace Lib.Hash.Hashable.ByteExtractors
{
    internal class DateTimeByteExtractor : IByteExtractor
    {
        private readonly Precision _precision;

        public enum Precision
        {
            Year,
            Month,
            Day,
            Hour,
            Minute,
            Second,
            Millisecond
        }

        public DateTimeByteExtractor(Precision precision = Precision.Millisecond)
        {
            _precision = precision;
        }

        public bool CanExtract(Type t) => t == typeof(DateTime);

        public void Extract(object instance, System.IO.Stream stream)
        {
            var dateTimeNullable = instance as DateTime?;
            Debug.Assert(dateTimeNullable != null, nameof(dateTimeNullable) + " != null");

            var dateTime = dateTimeNullable.Value;

            {
                var bytes = BitConverter.GetBytes(dateTime.Year);
                stream.Write(bytes, 0, bytes.Length);
            }
            if(_precision <= Precision.Year) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Month);
                stream.Write(bytes, 0, bytes.Length);
            }
            if (_precision <= Precision.Month) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Day);
                stream.Write(bytes, 0, bytes.Length);
            }
            if (_precision <= Precision.Day) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Hour);
                stream.Write(bytes, 0, bytes.Length);
            }
            if (_precision <= Precision.Hour) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Month);
                stream.Write(bytes, 0, bytes.Length);
            }
            if (_precision <= Precision.Minute) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Second);
                stream.Write(bytes, 0, bytes.Length);
            }
            if (_precision <= Precision.Second) return;

            {
                var bytes = BitConverter.GetBytes(dateTime.Millisecond);
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
