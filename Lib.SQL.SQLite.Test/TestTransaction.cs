using System;
using System.Collections.Generic;
using System.IO;
using Lib.SQL.Adapter;
using Microsoft.Data.Sqlite;
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

            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbPath) };
            var adapter = Adapter.Open(connString);
            Assert.AreEqual("committed", adapter.FetchValue("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public void TestRollback()
        {
            const string dbPath = "TestRollback.s3db";
            const string value = "committed";

            InsertValueInNewDb(value, dbPath, false);

            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbPath) };
            var adapter = Adapter.Open(connString);
            Assert.AreEqual((long) 0, adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private static void InsertValueInNewDb(string value, string dbName, bool commit)
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbName) };
            var adapter = Adapter.CreateFromPlainScript(connString, Resources.TestCommitRollback, true);

            adapter.ExecuteInTransaction(scope =>
            {
                adapter.Execute("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, object>{{"@value", value}});
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static int GetCount(ICommandChannel adapter)
        {
            return Convert.ToInt32(adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private static void InsertDoSomethingAndCommit(TransactionalDbAdapter adapter, string what, Action something)
        {
            var beforeCommit = 0;
            adapter.ExecuteInTransaction(scope =>
            {
                var initial = GetCount(scope);
                scope.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, GetCount(scope));

                something.Invoke();

                beforeCommit = GetCount(scope);
                return TransactionResult.Commit;
            });
            Assert.AreEqual(beforeCommit, GetCount(adapter));
        }

        private static void InsertDoSomethingAndRollback(TransactionalDbAdapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);
            adapter.ExecuteInTransaction(scope =>
            {
                scope.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, GetCount(scope));
                something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, GetCount(adapter));
        }

        private static void DoSomethingInsertAndCommit(TransactionalDbAdapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);

            adapter.ExecuteInTransaction(scope =>
            {
                something.Invoke();
                scope.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                return  TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, GetCount(adapter));
        }

        private static void DoSomethingInsertAndRollback(TransactionalDbAdapter adapter, string what, Action something)
        {
            var initial = GetCount(adapter);

            adapter.ExecuteInTransaction(scope =>
            {
                
                something.Invoke();

                var afterInvoke = GetCount(scope);
                scope.Execute("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(afterInvoke + 1, GetCount(scope));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, GetCount(adapter));
        }

        [TestMethod]
        public void TestNestedTransactions()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), "TestNestedTransactions.s3db") };
            var adapter = Adapter.CreateFromPlainScript(connString, Resources.TestCommitRollback, true);

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
