using Lib.SQL.Executor;
using Lib.SQL.Executor.Collections;
using Lib.SQL.QueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Tables.Test
{
    [TestClass]
    public class TestTable
    {
        [TestMethod]
        public void TestInsert()
        {
            var classicWay = Insert.Into("table", "colA", "colB").Values("a", 22);
            var dbalWay = new Table("table", "colA", "colB").Insert().Values("a", 22);

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var classicWay = Update.Table("table").Set("colA", 23);
            var dbalWay = new Table("table", "colA", "colB").Update().Set("colA", 23);

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestDelete()
        {
            var classicWay = Delete.From("table");
            var dbalWay = new Table("table", "colA", "colB").Delete();

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestSelect()
        {
            var classicWay = Select.AllFrom("table");
            var dbalWay = new Table("table", "colA", "colB").SelectCustom<SingleLineExecutor, ResultLine>();

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestSelectDistinct()
        {
            var classicWay = Select.AllFrom("table").Distinct();
            var dbalWay = new Table("table", "colA", "colB").SelectLine().Distinct();

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestSelectColumns()
        {
            var classicWay = Select.From("table", "colA");
            var dbalWay = new Table("table", "colA", "colB").SelectLines("colA");

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestSelectColumn()
        {
            var classicWay = Select.From("table", "colA");
            var dbalWay = new Table("table", "colA", "colB").SelectColumn("colA");

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestSelectColumnsDistinct()
        {
            var classicWay = Select.From("table", "colB").Distinct();
            var dbalWay = new Table("table", "colA", "colB").Select("colB").Distinct();

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }

        [TestMethod]
        public void TestExists()
        {
            var classicWay = Exists.InTable("table");
            var dbalWay = new Table("table", "colA", "colB").Exists();

            Assert.AreEqual(classicWay.Sql, dbalWay.Sql);
        }
    }
}
