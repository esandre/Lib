using Lib.SQL.QueryBuilder.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.QueryBuilder.Test
{
    [TestClass]
    public class TestSelect
    {
        [TestMethod]
        public void TestSelectAll()
        {
            var select = Select.AllFrom("test");
            Assert.AreEqual("SELECT * FROM test", select.Sql);
        }

        [TestMethod]
        public void TestSelectAllDistinct()
        {
            var select = Select.AllFrom("test").Distinct();
            Assert.AreEqual("SELECT DISTINCT * FROM test", select.Sql);
        }

        [TestMethod]
        public void TestSelectColumns()
        {
            var select = Select.From("test", "a", "b");
            Assert.AreEqual("SELECT a, b FROM test", select.Sql);
        }

        [TestMethod]
        public void TestSelectColumnsDistinct()
        {
            var select = Select.From("test", "a", "b").Distinct();
            Assert.AreEqual("SELECT DISTINCT a, b FROM test", select.Sql);
        }

        [TestMethod]
        public void TestLimit()
        {
            var select = Select.AllFrom("test").Limit(1);
            Assert.AreEqual("SELECT * FROM test LIMIT 1", select.Sql);
        }

        [TestMethod]
        public void TestLimitAndOffset()
        {
            var select = Select.AllFrom("test").Limit(1, 5);
            Assert.AreEqual("SELECT * FROM test LIMIT 1 OFFSET 5", select.Sql);
        }

        [TestMethod]
        public void TestOrder()
        {
            var select = Select.AllFrom("test").OrderBy("a", OrderDirection.Asc).OrderBy("b", OrderDirection.Desc);
            Assert.AreEqual("SELECT * FROM test ORDER BY a ASC, b DESC", select.Sql);
        }

        [TestMethod]
        public void TestGroupBy()
        {
            var select = Select.AllFrom("test").GroupBy("a");
            Assert.AreEqual("SELECT * FROM test GROUP BY a", select.Sql);
        }

        [TestMethod]
        public void TestGroupByAndHaving()
        {
            var select = Select.AllFrom("test").GroupBy("a", "a == 1");
            Assert.AreEqual("SELECT * FROM test GROUP BY a HAVING a == 1", select.Sql);
        }

        [TestMethod]
        public void TestJoin()
        {
            var select = Select.AllFrom("test")
                .Join("table", "table.a == test.a")
                .Join("table2", "table2.a == test.a", JoinType.Left)
                .Join("table3", "table3.a == table.a", JoinType.LeftOuter);

            Assert.AreEqual("SELECT * FROM test JOIN table ON (table.a == test.a) " +
                            "LEFT JOIN table2 ON (table2.a == test.a) " +
                            "LEFT OUTER JOIN table3 ON (table3.a == table.a)", select.Sql);
        }

    }
}
