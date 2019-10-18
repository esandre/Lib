using System;
using System.Collections.Generic;
using Lib.SQL.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.MySQL.Test
{
    [TestClass]
    public sealed class TestTransaction : TestAbstract
    {
        [TestMethod]
        public void TestCommit()
        {
            InsertValueInNewDb(true);

            var adapter = new Adapter(Credentials, "tmp");
            Assert.AreEqual("committed", adapter.FetchValue("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public void TestRollback()
        {
            InsertValueInNewDb(false);

            var adapter = new Adapter(Credentials, "tmp");
            Assert.AreEqual((long) 0, adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private static void InsertValueInNewDb(bool commit)
        {
            var adapter = Adapter.CreateFromPlainScript(Credentials, "tmp", Resources.TestCommitRollback, true);

            adapter.ExecuteInTransaction(() =>
            {
                adapter.Execute("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, object>{{"@value", "committed" } });
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static int GetCount(Table table)
        {
            return Convert.ToInt32(table.Select("COUNT(*)").Execute());
        }

        private static void InsertDoSomethingAndCommit(Table table, string what, Action something)
        {
            var beforeCommit = 0;
            ((TransactionalDbAdapter)table.Adapter()).ExecuteInTransaction(() =>
            {
                var initial = GetCount(table);
                table.Insert().Values(what).Execute();
                Assert.AreEqual(initial + 1, GetCount(table));

                something.Invoke();

                beforeCommit = GetCount(table);
                return TransactionResult.Commit;
            });
            Assert.AreEqual(beforeCommit, GetCount(table));
        }

        private static void InsertDoSomethingAndRollback(Table table, string what, Action something)
        {
            var initial = GetCount(table);
            ((TransactionalDbAdapter)table.Adapter()).ExecuteInTransaction(() =>
            {
                table.Insert().Values(what).Execute();
                Assert.AreEqual(initial + 1, GetCount(table));
                something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, GetCount(table));
        }

        private static void DoSomethingInsertAndCommit(Table table, string what, Action something)
        {
            var initial = GetCount(table);

            ((TransactionalDbAdapter)table.Adapter()).ExecuteInTransaction(() =>
            {
                something.Invoke();
                table.Insert().Values(what).Execute();
                return  TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, GetCount(table));
        }

        private static void DoSomethingInsertAndRollback(Table table, string what, Action something)
        {
            var initial = GetCount(table);

            ((TransactionalDbAdapter)table.Adapter()).ExecuteInTransaction(() =>
            {
                
                something.Invoke();

                var afterInvoke = GetCount(table);
                table.Insert().Values(what).Execute();
                Assert.AreEqual(afterInvoke + 1, GetCount(table));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, GetCount(table));
        }

        [TestMethod]
        public void TestNestedTransactions()
        {
            var adapter = Adapter.CreateFromPlainScript(Credentials, "tmp", Resources.TestCommitRollback, true);
            var table = new Table(() => adapter, "test", "reference");

            DoSomethingInsertAndCommit(table, "B",
                () => DoSomethingInsertAndRollback(table, "C",
                    () => InsertDoSomethingAndCommit(table, "D",
                        () => InsertDoSomethingAndRollback(table, "E", () => { })
                    )
                )
            );

            Assert.AreEqual(1, GetCount(table));
        }
    }
}
