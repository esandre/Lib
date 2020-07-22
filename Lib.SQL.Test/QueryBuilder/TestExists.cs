using System;
using Lib.SQL.Operation.QueryBuilder;
using Lib.SQL.Operation.QueryBuilder.Operator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
{
    [TestClass]
    public class TestExists
    {
        [TestMethod]
        public void Test()
        {
            var exists = Exists.InTable("test").Where("a", Is.GreaterThan, "b");
            Assert.AreEqual("SELECT EXISTS (SELECT 1 FROM test WHERE a > @" + Math.Abs("b".GetHashCode()) + ")", exists.Sql);
        }
    }
}
