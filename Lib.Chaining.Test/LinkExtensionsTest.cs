using Lib.Chaining.Extensions;
using Lib.Chaining.Test.Fixtures;
using Lib.Chaining.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.Chaining.Test
{
    [TestClass]
    public class LinkExtensionsTest : IReversibleEnumeratorImplicitBehaviorFixture
    {
        private IReversibleEnumeratorImplicitBehaviorFixture _enumeratorImplicitBehavior;

        [TestInitialize]
        public void Initialize()
        {
            _enumeratorImplicitBehavior = new ReversibleEnumeratorImplicitBehaviorFixtures<byte>(
                () => NumericalLineGenerator.GenerateFrom(128).ToReversibleEnumerable().GetEnumerator(), 
                byte.MaxValue - 127,
                129);
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
        public void Previous_ThenNext_IgnoresPrimer()
        {
            using (var enumerator = NumericalLineGenerator.GenerateFrom(128)
                .ToReversibleEnumerable()
                .GetEnumerator())
            {
                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.Current).Equals(129);
            }
        }

        [TestMethod]
        public void Next_ThenPrevious_IgnoresPrimer()
        {
            using (var enumerator = NumericalLineGenerator.GenerateFrom(128)
                .ToReversibleEnumerable()
                .GetEnumerator())
            {
                Check.That(enumerator.MoveNext()).IsTrue();
                Check.That(enumerator.MovePrevious()).IsTrue();
                Check.That(enumerator.Current).Equals(127);
            }
        }
    }
}
