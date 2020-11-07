using Lib.SQL.QueryBuilder.Operator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
{
    [TestClass]
    public class TestOperators
    {
        [TestMethod]
        public void TestPreset()
        {
            Assert.AreEqual("a IN (b,c)", Is.In.ToString("a", "b,c"));
            Assert.AreEqual("a = b", Is.EqualWith.ToString("a", "b"));
            Assert.AreEqual("a != b", Is.DifferentWith.ToString("a", "b"));
            Assert.AreEqual("a >= b", Is.GreaterOrEqualThan.ToString("a", "b"));
            Assert.AreEqual("a <= b", Is.LesserOrEqualThan.ToString("a", "b"));
            Assert.AreEqual("a > b", Is.GreaterThan.ToString("a", "b"));
            Assert.AreEqual("a < b", Is.LesserThan.ToString("a", "b"));
        }
    }
}
