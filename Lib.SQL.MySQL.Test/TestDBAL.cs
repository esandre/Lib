using System;
using Lib.SQL.Adapter;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.MySQL.Test
{
    [TestClass]
    public class TestDbal : TestAbstract
    {
        private DbAdapter _adapter;
        private readonly Table _table;

        [TestInitialize]
        public void Init()
        {
            _adapter = Adapter.CreateFromPlainScript(Credentials, "tmp", "CREATE TABLE example(colA TEXT, colB TEXT)", true);
        }

        public TestDbal()
        {
            _table = new Table(() => _adapter, "example", "colA", "colB");
        }

        [TestMethod]
        public void TestInsert()
        {
            var count = _table.Insert().Values("a", "b").Values("c", "d").Execute();
            Assert.AreEqual(2, count);

            var selected = _table.SelectLines().OrderBy("colA", OrderDirection.Asc).Execute();
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("a", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public void TestUpdate()
        {
            _table.Insert().Values("a", "b").Values("c", "d").Execute();
            _table.Update().Set("colA", "z").Where("colB", Is.EqualWith, "b").Execute();

            var selected = _table.SelectLines().OrderBy("colB", OrderDirection.Asc).Execute();
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("z", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public void TestDelete()
        {
            _table.Insert().Values("a", "b").Values("c", "d").Execute();
            _table.Delete().Where("colB", Is.EqualWith, "b").Execute();

            var selected = _table.SelectLines().Execute();
            Assert.AreEqual(1, selected.Count);
            Assert.AreEqual("c", selected[0]["colA"].ToString());
            Assert.AreEqual("d", selected[0]["colB"].ToString());
        }

        [TestMethod]
        public void TestSelect()
        {
            var rowId = _table.Insert().Values("a", "b").Values("c", "d").ExecuteAndReturnRowId();
            Assert.AreEqual(rowId, Convert.ToInt64(_table.Select("LAST_INSERT_ID()").Execute()));
        }

        [TestMethod]
        public void TestExists()
        {
            var selected = _table.Exists().Execute();
            Assert.AreEqual(false, selected);

            _table.Insert().Values("a", "b").Values("c", "d").Execute();
            selected = _table.Exists().Where("colB", Is.EqualWith, "b").Execute();
            Assert.AreEqual(true, selected);
        }

        [TestMethod]
        public void TestLastInsertedId()
        {
            var notInserted = _table.LastInsertedId;
            Assert.AreEqual(0, notInserted);

            var id = _table.Insert().Values("a", "b").ExecuteAndReturnRowId();

            var lastInserted = _table.LastInsertedId;
            Assert.AreEqual(id, lastInserted);
        }
    }
}
