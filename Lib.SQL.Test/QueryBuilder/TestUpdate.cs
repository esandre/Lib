using System;
using System.Collections.Generic;
using Lib.SQL.Operation.QueryBuilder;
using Lib.SQL.Operation.QueryBuilder.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
{
    [TestClass]
    public class TestUpdate
    {
        [TestMethod]
        public void TestSimpleUpdate()
        {
            var update = Update.Table("test").Set("a", "b");
            Assert.AreEqual("UPDATE test SET a = @" + Math.Abs("b".GetHashCode()), update.Sql);
        }

        [TestMethod]
        public void TestOnError()
        {
            var update = Update.Table("test").Set("a", "b").OnError(OrType.Fail);
            Assert.AreEqual("UPDATE OR FAIL test SET a = @" + Math.Abs("b".GetHashCode()), update.Sql);
        }

        [TestMethod]
        public void TestRedefineColumn()
        {
            var update = Update.Table("test").Set("a", "b").Set("a", "c");
            Assert.AreEqual("UPDATE test SET a = @" + Math.Abs("c".GetHashCode()), update.Sql);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoColumnUpdate()
        {
            Assert.IsNotNull(Update.Table("test").Sql);
        }

        [TestMethod]
        public void TestMultipleColumns()
        {
            var columns = new Dictionary<string, IConvertible>
            {
                {"a", "b"},
                {"c", "d"}
            };

            var update = Update.Table("test").Set(columns);
            Assert.AreEqual(string.Format("UPDATE test SET a = @{0}, c = @{1}", Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode())), update.Sql);
        }
    }
}
