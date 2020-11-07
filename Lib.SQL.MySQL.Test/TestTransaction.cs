using System;
using System.Collections.Generic;
using Lib.SQL.Adapter;
using Lib.SQL.Tables;
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

            var adapter = Adapter.Open(Credentials);
            Assert.AreEqual("committed", adapter.FetchValue("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public void TestRollback()
        {
            InsertValueInNewDb(false);

            var adapter = Adapter.Open(Credentials);
            Assert.AreEqual((long) 0, adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private void InsertValueInNewDb(bool commit)
        {
            var adapter = Adapter.CreateFromPlainScript(Credentials, Resources.TestCommitRollback, true);

            adapter.ExecuteInTransaction(scope =>
            {
                scope.Execute("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, object>{{"@value", "committed" } });
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static int GetCount(Table table, ICommandChannel adapter)
        {
            return Convert.ToInt32(table.Select("COUNT(*)").ExecuteOn(adapter));
        }

        private static void InsertDoSomethingAndCommit(TransactionalDbAdapter adapter, Table table, string what, Action something)
        {
            var beforeCommit = 0;
            adapter.ExecuteInTransaction(scope =>
            {
                var initial = GetCount(table, scope);
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(initial + 1, GetCount(table, scope));

                something.Invoke();

                beforeCommit = GetCount(table, scope);
                return TransactionResult.Commit;
            });
            Assert.AreEqual(beforeCommit, GetCount(table, adapter));
        }

        private static void InsertDoSomethingAndRollback(TransactionalDbAdapter adapter, Table table, string what, Action something)
        {
            var initial = GetCount(table, adapter);
            adapter.ExecuteInTransaction(scope =>
            {
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(initial + 1, GetCount(table, scope));
                something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, GetCount(table, adapter));
        }

        private static void DoSomethingInsertAndCommit(TransactionalDbAdapter adapter, Table table, string what, Action something)
        {
            var initial = GetCount(table, adapter);

            adapter.ExecuteInTransaction(scope =>
            {
                something.Invoke();
                table.Insert().Values(what).ExecuteOn(scope);
                return  TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, GetCount(table, adapter));
        }

        private static void DoSomethingInsertAndRollback(TransactionalDbAdapter adapter, Table table, string what, Action something)
        {
            var initial = GetCount(table, adapter);

            adapter.ExecuteInTransaction(scope =>
            {
                something.Invoke();

                var afterInvoke = GetCount(table, scope);
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(afterInvoke + 1, GetCount(table, scope));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, GetCount(table, adapter));
        }

        [TestMethod]
        public void TestNestedTransactions()
        {
            var adapter = Adapter.CreateFromPlainScript(Credentials, Resources.TestCommitRollback, true);
            var table = new Table("test", "reference");

            DoSomethingInsertAndCommit(adapter, table, "B",
                () => DoSomethingInsertAndRollback(adapter, table, "C",
                    () => InsertDoSomethingAndCommit(adapter, table, "D",
                        () => InsertDoSomethingAndRollback(adapter, table, "E", () => { })
                    )
                )
            );

            Assert.AreEqual(1, GetCount(table, adapter));
        }
    }
}
