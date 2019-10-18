using System;
using System.Collections.Generic;
using System.IO;
using Lib.SQL.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.SQLite.Test
{
    [TestClass]
    public sealed class TestTransaction
    {
        [TestMethod]
        public void TestCommit()
        {
            const string dbPath = "TestCommit.s3db";
            const string value = "committed";

            InsertValueInNewDb(value, dbPath, true);

            var adapter = Adapter.Open(Path.Combine(Path.GetTempPath(), dbPath));
            Assert.AreEqual("committed", adapter.FetchValue("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public void TestRollback()
        {
            const string dbPath = "TestRollback.s3db";
            const string value = "committed";

            InsertValueInNewDb(value, dbPath, false);

            var adapter = Adapter.Open(Path.Combine(Path.GetTempPath(), dbPath));
            Assert.AreEqual((long) 0, adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private static void InsertValueInNewDb(string value, string dbName, bool commit)
        {
            var script = Path.Combine(Path.GetTempPath(), dbName);
            var adapter = Adapter.CreateFromPlainScript(script, Resources.TestCommitRollback, true);

            adapter.ExecuteInTransaction(() =>
            {
                adapter.Execute("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, object>{{"@value", value}});
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static int GetCount(DbAdapter adapter)
        {
            return Convert.ToInt32(adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private static void InsertDoSomethingAndCommit(TransactionalDbAdapter adapter, string what, Action something)
        {
            var beforeCommit = 0;
            adapter.ExecuteInTransaction(() =>
            {
                var initial = GetCount(adapter);
                adapter.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, GetCount(adapter));

                something.Invoke();

                beforeCommit = GetCount(adapter);
                return TransactionResult.Commit;
            });
            Assert.AreEqual(beforeCommit, GetCount(adapter));
        }

        private static void InsertDoSomethingAndRollback(Adapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);
            adapter.ExecuteInTransaction(() =>
            {
                adapter.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, GetCount(adapter));
                something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, GetCount(adapter));
        }

        private static void DoSomethingInsertAndCommit(TransactionalDbAdapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);

            adapter.ExecuteInTransaction(() =>
            {
                something.Invoke();
                adapter.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                return  TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, GetCount(adapter));
        }

        private static void DoSomethingInsertAndRollback(TransactionalDbAdapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);

            adapter.ExecuteInTransaction(() =>
            {
                
                something.Invoke();

                var afterInvoke = GetCount(adapter);
                adapter.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(afterInvoke + 1, GetCount(adapter));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, GetCount(adapter));
        }

        [TestMethod]
        public void TestNestedTransactions()
        {
            var script = Path.Combine(Path.GetTempPath(), "TestNestedTransactions.s3db");
            var adapter = Adapter.CreateFromPlainScript(script, Resources.TestCommitRollback, true);

            DoSomethingInsertAndCommit(adapter, "B",
                () => DoSomethingInsertAndRollback(adapter, "C",
                    () => InsertDoSomethingAndCommit(adapter, "D",
                        () => InsertDoSomethingAndRollback(adapter, "E", () => { })
                    )
                )
            );

            Assert.AreEqual(1, GetCount(adapter));
        }
    }
}
