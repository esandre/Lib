using System;
using System.Collections.Generic;
using Lib.SQL.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace Lib.SQL.MySQL.Test
{
    [TestClass]
    public sealed class TestTransaction : TestAbstract
    {
        [TestMethod]
        public void TestCommit()
        {
            InsertValueInNewDb(true);

            var adapter = new MySQLCommandChannelFactory().Open(Credentials);
            Assert.AreEqual("committed", adapter.FetchValue("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public void TestRollback()
        {
            InsertValueInNewDb(false);

            var adapter = new MySQLCommandChannelFactory().Open(Credentials);
            Assert.AreEqual((long) 0, adapter.FetchValue("SELECT COUNT(*) FROM test"));
        }

        private void InsertValueInNewDb(bool commit)
        {
            var adapter = new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCommitRollback, true));

            adapter.ExecuteInTransaction(scope =>
            {
                scope.Execute("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, IConvertible>{{"@value", "committed" } });
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static int GetCount(Table table, ICommandChannel adapter)
        {
            return Convert.ToInt32(table.Select("COUNT(*)").ExecuteOn(adapter));
        }

        private static void InsertDoSomethingAndCommit(ICommandChannel commandChannel, Table table, string what, Action something)
        {
            var beforeCommit = 0;
            commandChannel.ExecuteInTransaction(scope =>
            {
                var initial = GetCount(table, scope);
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(initial + 1, GetCount(table, scope));

                something.Invoke();

                beforeCommit = GetCount(table, scope);
                return TransactionResult.Commit;
            });
            Assert.AreEqual(beforeCommit, GetCount(table, commandChannel));
        }

        private static void InsertDoSomethingAndRollback(ICommandChannel commandChannel, Table table, string what, Action something)
        {
            var initial = GetCount(table, commandChannel);
            commandChannel.ExecuteInTransaction(scope =>
            {
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(initial + 1, GetCount(table, scope));
                something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, GetCount(table, commandChannel));
        }

        private static void DoSomethingInsertAndCommit(ICommandChannel commandChannel, Table table, string what, Action something)
        {
            var initial = GetCount(table, commandChannel);

            commandChannel.ExecuteInTransaction(scope =>
            {
                something.Invoke();
                table.Insert().Values(what).ExecuteOn(scope);
                return  TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, GetCount(table, commandChannel));
        }

        private static void DoSomethingInsertAndRollback(ICommandChannel commandChannel, Table table, string what, Action something)
        {
            var initial = GetCount(table, commandChannel);

            commandChannel.ExecuteInTransaction(scope =>
            {
                something.Invoke();

                var afterInvoke = GetCount(table, scope);
                table.Insert().Values(what).ExecuteOn(scope);
                Assert.AreEqual(afterInvoke + 1, GetCount(table, scope));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, GetCount(table, commandChannel));
        }

        [TestMethod]
        public void TestNestedTransactions()
        {
            var adapter = new MySQLCommandChannelFactory().Create(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCommitRollback, true));
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
