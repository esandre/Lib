using System;
using Lib.Chaining.Chain;
using Lib.Chaining.Structures;
using Moq;

namespace Lib.Chaining.Test.Utils
{
    public class NumericalSegmentGenerator : NumericalLineGenerator
    {
        private readonly byte _start;
        private readonly byte _end;

        public static ISegment<byte> GenerateSegment(byte start, byte end)
        {
            if(start == end) throw new ArgumentException("A segment with Start and End being the same link is impossible.");
            if (start > end) return GenerateSegment(end, start);

            var generator = new NumericalSegmentGenerator(start, end);
            var startLink = (IPredecessor<byte>) generator.From(start);
            var endLink = (ISuccessor<byte>) generator.From(end);

            return Mock.Of<ISegment<byte>>(m => m.Start == startLink && m.End == endLink);
        }

        private NumericalSegmentGenerator(byte start, byte end)
        {
            _start = start;
            _end = end;
        }

        protected override bool HasPrevious(byte id) => id > _start;
        protected override bool HasNext(byte id) => id < _end;
    }
}
