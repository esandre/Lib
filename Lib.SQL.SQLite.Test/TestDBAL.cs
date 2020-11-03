﻿using Lib.SQL.Adapter;
using Lib.SQL.Operation.QueryBuilder.Operator;
using Lib.SQL.Operation.QueryBuilder.Sequences;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.SQLite.Test
{
    [TestClass]
    public class TestDbal
    {
        private readonly DbAdapter _adapter =  Adapter.CreateFromPlainScript(new SqliteConnectionStringBuilder { DataSource = ":memory:" }, "CREATE TABLE example(colA TEXT, colB TEXT)", true);
        private readonly Table _table;

        public TestDbal()
        {
            _table = new Table("example", "colA", "colB");
        }

        [TestMethod]
        public void TestInsert()
        {
            var count = _table.Insert().Values("a", "b").Values("c", "d").ExecuteOn(_adapter);
            Assert.AreEqual(2, count);

            var selected = _table.SelectLines().OrderBy("colA", OrderDirection.Asc).ExecuteOn(_adapter);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("a", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public void TestUpdate()
        {
            _table.Insert().Values("a", "b").Values("c", "d").ExecuteOn(_adapter);
            _table.Update().Set("colA", "z").Where("colB", Is.EqualWith, "b").ExecuteOn(_adapter);

            var selected = _table.SelectLines().OrderBy("colB", OrderDirection.Asc).ExecuteOn(_adapter);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("z", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public void TestDelete()
        {
            _table.Insert().Values("a", "b").Values("c", "d").ExecuteOn(_adapter);
            _table.Delete().Where("colB", Is.EqualWith, "b").ExecuteOn(_adapter);

            var selected = _table.SelectLines().ExecuteOn(_adapter);
            Assert.AreEqual(1, selected.Count);
            Assert.AreEqual("c", selected[0]["colA"].ToString());
            Assert.AreEqual("d", selected[0]["colB"].ToString());
        }

        [TestMethod]
        public void TestSelect()
        {
            var rowId = _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAndReturnRowId(_adapter);
            Assert.AreEqual(rowId, _table.Select("last_insert_rowid()").ExecuteOn(_adapter));
        }

        [TestMethod]
        public void TestExists()
        {
            var selected = _table.Exists().ExecuteOn(_adapter);
            Assert.AreEqual(false, selected);

            _table.Insert().Values("a", "b").Values("c", "d").ExecuteOn(_adapter);
            selected = _table.Exists().Where("colB", Is.EqualWith, "b").ExecuteOn(_adapter);
            Assert.AreEqual(true, selected);
        }

        [TestMethod]
        public void TestLastInsertedId()
        {
            var notInserted = _adapter.LastInsertedId;
            Assert.AreEqual(0, notInserted);

            var id = _table.Insert().Values("a", "b").ExecuteOnAndReturnRowId(_adapter);

            var lastInserted = _adapter.LastInsertedId;
            Assert.AreEqual(id, lastInserted);
        }
    }
}
