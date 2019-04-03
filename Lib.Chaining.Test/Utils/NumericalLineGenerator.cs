using System;
using Lib.Chaining.Chain;
using Moq;

namespace Lib.Chaining.Test.Utils
{
    public class NumericalLineGenerator
    {
        public static ILink<byte> GenerateFrom(byte origin) => new NumericalLineGenerator().From(origin);

        protected NumericalLineGenerator() { }

        protected ILink<byte> From(byte origin)
        {
            var mock = new Mock<ILink<byte>>();
            mock.Setup(m => m.Payload).Returns(origin);

            SetupAsPredecessorIfNeeded(origin, mock);
            SetupAsSuccessorIfNeeded(origin, mock);

            return mock.Object;
        }

        protected virtual bool HasNext(byte id) => id < byte.MaxValue;
        protected virtual bool HasPrevious(byte id) => id > byte.MinValue;

        private Func<IPredecessor<byte>, ISuccessor<byte>> GenerateNext => predecessor =>
        {
            var payload = (byte) (predecessor.Payload + 1);

            var mock = new Mock<ISuccessor<byte>>();
            mock.Setup(m => m.Payload).Returns(payload);
            mock.Setup(m => m.Previous).Returns(predecessor);

            SetupAsPredecessorIfNeeded(payload, mock);

            return mock.Object;
        };

        private Func<ISuccessor<byte>, IPredecessor<byte>> GeneratePrevious => successor =>
        {
            var payload = (byte) (successor.Payload - 1);

            var mock = new Mock<IPredecessor<byte>>();
            mock.Setup(m => m.Payload).Returns(payload);
            mock.Setup(m => m.Next).Returns(successor);

            SetupAsSuccessorIfNeeded(payload, mock);

            return mock.Object;
        };

        private void SetupAsPredecessorIfNeeded(byte payload, Mock mock)
        {
            if (!HasNext(payload)) return;
            var asPredecessor = mock.As<IPredecessor<byte>>();
            asPredecessor.SetupGet(m => m.Next).Returns(() => GenerateNext(asPredecessor.Object));
        }

        private void SetupAsSuccessorIfNeeded(byte payload, Mock mock)
        {
            if (!HasPrevious(payload)) return;
            var asSuccessor = mock.As<ISuccessor<byte>>();
            asSuccessor.SetupGet(m => m.Previous).Returns(() => GeneratePrevious(asSuccessor.Object));
        }
    }
}
