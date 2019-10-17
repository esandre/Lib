using System;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
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
