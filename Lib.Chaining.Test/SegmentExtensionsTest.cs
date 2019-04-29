using Lib.Chaining.Chain;
using Lib.Chaining.Extensions;
using Lib.Chaining.Structures;
using Lib.Chaining.Test.Fixtures;
using Lib.Chaining.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Chaining.Test
{
    [TestClass]
    public class SegmentExtensionsTest : IReversibleEnumeratorImplicitBehaviorFixture
    {
        private IReversibleEnumeratorImplicitBehaviorFixture _enumeratorImplicitBehavior;

        [TestInitialize]
        public void Initialize()
        {
            var segment = NumericalSegmentGenerator.GenerateSegment(0, 10).ToReadonlyReversibleCollection();
            _enumeratorImplicitBehavior = new ReversibleEnumeratorImplicitBehaviorFixture<byte>(
                () => segment.GetEnumerator(), 
                (byte) segment.Count,
                (byte) segment.Count);
        }

        [TestMethod]
        public void Current_Initially_ReturnsDefault() =>
            _enumeratorImplicitBehavior.Current_Initially_ReturnsDefault();

        [TestMethod]
        public void Reset_SetsCurrent_ToDefault() =>
            _enumeratorImplicitBehavior.Reset_SetsCurrent_ToDefault();

        [TestMethod]
        public void NextReturnsFalse_AtTheEnd() => 
            _enumeratorImplicitBehavior.NextReturnsFalse_AtTheEnd();

        [TestMethod]
        public void NextNeverThrows_AfterTheEnd() => 
            _enumeratorImplicitBehavior.NextNeverThrows_AfterTheEnd();

        [TestMethod]
        public void PreviousReturnsFalse_AtTheBeginning() => 
            _enumeratorImplicitBehavior.PreviousReturnsFalse_AtTheBeginning();

        [TestMethod]
        public void PreviousNeverThrows_BeforeTheBeginning() => 
            _enumeratorImplicitBehavior.PreviousNeverThrows_BeforeTheBeginning();

        [TestMethod]
        public void NextAndPrevious_Commutative() => 
            _enumeratorImplicitBehavior.NextAndPrevious_Commutative();

        [TestMethod]
        public void SegmentCount_SameInCollection()
        {
            Check.That(NumericalSegmentGenerator.GenerateSegment(1, 10).ToReadonlyReversibleCollection().Count).Equals(10);
            Check.That(NumericalSegmentGenerator.GenerateSegment(0, 1).ToReadonlyReversibleCollection().Count).Equals(2);
            Check.That(NumericalSegmentGenerator.GenerateSegment(7, 1).ToReadonlyReversibleCollection().Count).Equals(7);
        }

        [TestMethod]
        public void Previous_ThenNext_ReturnsDefault()
        {
            using (var enumerator = NumericalSegmentGenerator.GenerateSegment(0, 10)
                .ToReadonlyReversibleCollection()
                .GetEnumerator())
            {
                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.MoveNext()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        [TestMethod]
        public void Next_ThenPrevious_ReturnsDefault()
        {
            using (var enumerator = NumericalSegmentGenerator.GenerateSegment(0, 10)
                .ToReadonlyReversibleCollection()
                .GetEnumerator())
            {
                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.MovePrevious()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        [TestMethod]
        public void BoundariesEnforced_EvenIfStartIsSuccessor()
        {
            var line = NumericalLineGenerator.GenerateFrom(128);
            var start = (IPredecessor<byte>) line;
            var end = start.Next;

            var segment = Mock.Of<ISegment<byte>>(m => m.Start == start && m.End == end);
            var collection = segment.ToReadonlyReversibleCollection();

            Check.That(collection.Count).Equals(2);

            using (var enumerator = collection.GetEnumerator())
            {
                Check.That(enumerator.Current).IsDefaultValue();

                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(128);

                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(129);

                Check.That(enumerator.MoveNext()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();

                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(129);

                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(128);

                Check.That(enumerator.MovePrevious()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }

        [TestMethod]
        public void BoundariesEnforced_EvenIfEndIsPredecessor()
        {
            var line = NumericalLineGenerator.GenerateFrom(128);
            var end = (ISuccessor<byte>)line;
            var start = end.Previous;

            var segment = Mock.Of<ISegment<byte>>(m => m.Start == start && m.End == end);
            var collection = segment.ToReadonlyReversibleCollection();

            Check.That(collection.Count).Equals(2);

            using (var enumerator = collection.GetEnumerator())
            {
                Check.That(enumerator.Current).IsDefaultValue();

                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(128);

                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(127);

                Check.That(enumerator.MovePrevious()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();

                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(127);

                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(128);

                Check.That(enumerator.MoveNext()).IsFalse();
                Check.That(enumerator.Current).IsDefaultValue();
            }
        }
    }
}
