﻿using System.Threading.Tasks;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences;
using Lib.SQL.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace Lib.SQL.SQLite.Test
{
    [TestClass]
    public class TestDbal
    {
        private readonly ICommandChannel _adapter = new MemorySqliteCommandChannelFactory()
            .Create(
                new CreationParameters<MemorySqliteConnectionStringBuilder>(new MemorySqliteConnectionStringBuilder(), "CREATE TABLE example(colA TEXT, colB TEXT)", true)
                );
        
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
            var selected = _table.Exists().Where("1", Is.DifferentWith, "1").ExecuteOn(_adapter);
            Assert.AreEqual(false, selected);

            _table.Insert().Values("a", "b").Values("c", "d").ExecuteOn(_adapter);
            selected = _table.Exists().Where("colB", Is.EqualWith, "b").ExecuteOn(_adapter);
            Assert.AreEqual(true, selected);
        }

        [TestMethod]
        public void TestLastInsertedId()
        {
            var adapter = new MemorySqliteCommandChannelFactory()
                .Create(
                    new CreationParameters<MemorySqliteConnectionStringBuilder>(new MemorySqliteConnectionStringBuilder(), "CREATE TABLE example(colA INT AUTO_INCREMENT, colB TEXT)", true)
                );

            var notInserted = adapter.LastInsertedId;
            Assert.AreEqual((long) 0, notInserted);

            var id = _table.Insert().Values(null, "b").ExecuteOnAndReturnRowId(adapter);

            var lastInserted = adapter.LastInsertedId;
            Assert.AreEqual(id, lastInserted);
        }

        [TestMethod]
        public async Task TestLastInsertedIdAsync()
        {
            var adapter = await new MemorySqliteCommandChannelFactory()
                .CreateAsync(
                    new CreationParameters<MemorySqliteConnectionStringBuilder>(new MemorySqliteConnectionStringBuilder(), "CREATE TABLE example(colA INT AUTO_INCREMENT, colB TEXT)", true)
                );

            var notInserted = await adapter.LastInsertedIdAsync();
            Assert.AreEqual((long)0, notInserted);

            var id = await  _table.Insert().Values(null, "b").ExecuteOnAndReturnRowIdAsync(adapter);
            var lastInserted = await adapter.LastInsertedIdAsync();

            Assert.AreEqual(id, lastInserted);
            Assert.AreNotEqual(id, notInserted);
        }
    }
}
