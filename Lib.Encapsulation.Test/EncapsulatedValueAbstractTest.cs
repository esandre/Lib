using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.Encapsulation.Test
{
    [TestClass]
    public class EncapsulatedValueAbstractTest
    {
        [TestMethod]
        public void BuildFromOther_TakesOtherValue()
        {
            var value = new object();
            var other = Mock.Of<IEncapsulatedValue<object>>(m => m.Value == value);

            var instance = new Mock<EncapsulatedValueAbstract<object>>(other) { CallBase = true }.Object;

            Check.That(instance.Value).Equals(value);
        }

        [TestMethod]
        public void BuildFromValue_TakesValue()
        {
            var value = new object();
            var instance = new Mock<EncapsulatedValueAbstract<object>>(value) { CallBase = true }.Object;

            Check.That(instance.Value).Equals(value);
        }

        [TestMethod]
        public void ToString_ReturnsValueToString()
        {
            var value = Mock.Of<object>(m => m.ToString() == "x");
            var instance = new Mock<EncapsulatedValueAbstract<object>>(value) { CallBase = true }.Object;

            Check.That(instance.ToString()).Equals(value.ToString());
        }

        [TestMethod]
        public void TwoChildren_OfSameType_WithSameContent_AreEquals()
        {
            var x = new ChildClass("x");
            var y = new ChildClass("x");

            Check.That(Equals(x, y)).IsTrue();
            Check.That(x == y).IsTrue();
            Check.That(x.Equals(y)).IsTrue();
            Check.That(y.Equals(x)).IsTrue();
        }

        private class ChildClass : EncapsulatedValueAbstract<object>
        {
            public ChildClass(IEncapsulatedValue<object> other) : base(other)
            {
            }

            public ChildClass(object input) : base(input)
            {
            }
        }
    }
}
