using System;

namespace Lib.Random.Test
{
    internal class FakeRandom : System.Random
    {
        public override int Next() => default(int);

        public override int Next(int maxValue) => default(int);

        public override int Next(int minValue, int maxValue) => minValue;

        public override void NextBytes(byte[] buffer)
        {
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = default(byte);
        }

        public override void NextBytes(Span<byte> buffer)
        {
            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = default(byte);
        }

        public override double NextDouble() => default(double);
    }
}
