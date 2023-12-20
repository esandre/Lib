using System.Threading.Tasks;
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
        private IAsyncCommandChannel _adapter;
        
        private readonly Table _table = new("example", "colA", "colB");

        [TestInitialize]
        public async Task Initialize()
        {
            _adapter = await new MemorySqliteCommandChannelFactory()
                .CreateAsync(
                    new CreationParameters<MemorySqliteConnectionStringBuilder>(new MemorySqliteConnectionStringBuilder(), 
                        "CREATE TABLE example(colA TEXT, colB TEXT)", true)
                );
        }

        [TestMethod]
        public async Task TestInsert()
        {
            var count = await _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAsync(_adapter);
            Assert.AreEqual(2, count);

            var selected = await _table.SelectLines().OrderBy("colA", OrderDirection.Asc).ExecuteOnAsync(_adapter);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("a", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            await _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAsync(_adapter);
            await _table.Update().Set("colA", "z").Where("colB", Is.EqualWith, "b").ExecuteOnAsync(_adapter);

            var selected = await _table.SelectLines().OrderBy("colB", OrderDirection.Asc).ExecuteOnAsync(_adapter);
            Assert.AreEqual(2, selected.Count);
            Assert.AreEqual("z", selected[0]["colA"].ToString());
            Assert.AreEqual("b", selected[0]["colB"].ToString());
            Assert.AreEqual("c", selected[1]["colA"].ToString());
            Assert.AreEqual("d", selected[1]["colB"].ToString());
        }

        [TestMethod]
        public async Task TestDelete()
        {
            await _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAsync(_adapter);
            await _table.Delete().Where("colB", Is.EqualWith, "b").ExecuteOnAsync(_adapter);

            var selected = await _table.SelectLines().ExecuteOnAsync(_adapter);
            Assert.AreEqual(1, selected.Count);
            Assert.AreEqual("c", selected[0]["colA"].ToString());
            Assert.AreEqual("d", selected[0]["colB"].ToString());
        }

        [TestMethod]
        public async Task TestSelect()
        {
            var rowId = await _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAndReturnRowIdAsync(_adapter);
            Assert.AreEqual(rowId, await _table.Select("last_insert_rowid()").ExecuteOnAsync(_adapter));
        }

        [TestMethod]
        public async Task TestExists()
        {
            var selected = await _table.Exists().Where("1", Is.DifferentWith, "1").ExecuteOnAsync(_adapter);
            Assert.AreEqual(false, selected);

            await _table.Insert().Values("a", "b").Values("c", "d").ExecuteOnAsync(_adapter);
            selected = await _table.Exists().Where("colB", Is.EqualWith, "b").ExecuteOnAsync(_adapter);
            Assert.AreEqual(true, selected);
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
