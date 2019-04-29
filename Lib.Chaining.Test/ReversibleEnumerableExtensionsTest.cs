using Lib.Chaining.Extensions;
using Lib.Chaining.Test.Fixtures;
using Lib.Chaining.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Chaining.Test
{
    [TestClass]
    public class ReversibleEnumerableExtensionsTest : IEnumeratorImplicitBehaviorFixture
    {
        private IReversibleEnumeratorImplicitBehaviorFixture _enumeratorImplicitBehavior;

        [TestInitialize]
        public void Initialize()
        {
            _enumeratorImplicitBehavior = 
                new ReversibleEnumeratorImplicitBehaviorFixture<byte>(
                    () => NumericalLineGenerator.GenerateFrom(128).ToReversibleEnumerable().Narrow<byte>(126, 130).GetEnumerator(), 
                    3, 3);
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
    }
}
