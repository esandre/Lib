using Lib.Chaining.Chain;
using Lib.Chaining.Structures;
using Moq;

namespace Lib.Chaining.Test.Utils
{
    public class NumericalRayGenerator : NumericalLineGenerator
    {
        private readonly byte _start;
        private readonly byte _limit;

        public static IRay<byte> GenerateRay(byte start = 0, byte limit = byte.MaxValue) 
            => Mock.Of<IRay<byte>>(m => m.Origin == (IPredecessor<byte>) new NumericalRayGenerator(start, limit).From(start));

        private NumericalRayGenerator(byte start, byte limit = byte.MaxValue)
        {
            _start = start;
            _limit = limit;
        }

        protected override bool HasPrevious(byte id) => id > _start;

        protected override bool HasNext(byte id) => id < _limit;
    }
}
