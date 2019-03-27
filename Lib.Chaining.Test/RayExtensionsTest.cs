using Lib.Chaining.Extensions;
using Lib.Chaining.Test.Fixtures;
using Lib.Chaining.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Chaining.Test
{
    [TestClass]
    public class RayExtensionsTest : IEnumeratorImplicitBehaviorFixture
    {
        private IEnumeratorImplicitBehaviorFixture _enumeratorImplicitBehavior;

        [TestInitialize]
        public void Initialize()
        {
            _enumeratorImplicitBehavior = new EnumeratorImplicitBehaviorFixture<byte>(() => NumericalRayGenerator.GenerateRay(0, 10).ToEnumerable().GetEnumerator(), 11);
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
    }
}
