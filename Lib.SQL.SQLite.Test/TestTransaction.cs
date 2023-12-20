using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.SQLite.Test
{
    [TestClass]
    public sealed class TestTransaction
    {
        [TestMethod]
        public async Task TestCommit()
        {
            const string dbPath = "TestCommit.s3db";
            const string value = "committed";

            await InsertValueInNewDbAsync(value, dbPath, true);

            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbPath) };
            var adapter = new SqliteCommandChannelFactory().OpenAsync(connString);
            Assert.AreEqual("committed", await adapter.FetchValueAsync("SELECT reference FROM test LIMIT 1"));
        }

        [TestMethod]
        public async Task TestRollback()
        {
            const string dbPath = "TestRollback.s3db";
            const string value = "committed";

            await InsertValueInNewDbAsync(value, dbPath, false);

            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbPath) };
            var adapter = new SqliteCommandChannelFactory().OpenAsync(connString);
            Assert.AreEqual((long) 0, await adapter.FetchValueAsync("SELECT COUNT(*) FROM test"));
        }

        private static async Task InsertValueInNewDbAsync(string value, string dbName, bool commit)
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), dbName) };
            var adapter = await new SqliteCommandChannelFactory()
                .CreateAsync(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCommitRollback, true));

            await adapter.ExecuteInTransactionAsync(async _ =>
            {
                await adapter.ExecuteAsync("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, IConvertible>{{"@value", value}});
                return commit ? TransactionResult.Commit : TransactionResult.Rollback;
            });
        }

        private static async Task<int> GetCountAsync(IAsyncCommandChannel adapter)
        {
            return Convert.ToInt32(await adapter.FetchValueAsync("SELECT COUNT(*) FROM test"));
        }

        private static async Task InsertDoSomethingAndCommit(IAsyncCommandChannel adapter, string what, Func<Task> something)
        {
            var beforeCommit = 0;
            await adapter.ExecuteInTransactionAsync(async scope =>
            {
                var initial = await GetCountAsync(scope);
                await scope.ExecuteAsync("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, await GetCountAsync(scope));

                await something.Invoke();

                beforeCommit = await GetCountAsync(scope);
                return TransactionResult.Commit;
            });

            Assert.AreEqual(beforeCommit, await GetCountAsync(adapter));
        }

        private static async Task InsertDoSomethingAndRollback(IAsyncCommandChannel adapter, string what, Func<Task> something)
        {
            var initial = await GetCountAsync(adapter);
            await adapter.ExecuteInTransactionAsync(async scope =>
            {
                await scope.ExecuteAsync("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(initial + 1, await GetCountAsync(scope));
                await something.Invoke();
                return TransactionResult.Rollback;
            });
            Assert.AreEqual(initial, await GetCountAsync(adapter));
        }

        private static async Task DoSomethingInsertAndCommit(IAsyncCommandChannel adapter, string what, Func<Task> something)
        {
            var initial = await GetCountAsync(adapter);

            await adapter.ExecuteInTransactionAsync(async scope =>
            {
                await something.Invoke();
                await scope.ExecuteAsync("INSERT INTO test (reference) VALUES ('" + what + "')");
                return TransactionResult.Commit;
            });

            Assert.AreEqual(initial + 1, await GetCountAsync(adapter));
        }

        private static async Task DoSomethingInsertAndRollback(IAsyncCommandChannel adapter, string what, Func<Task> something)
        {
            var initial = await GetCountAsync(adapter);

            await adapter.ExecuteInTransactionAsync(async scope =>
            {
                await something.Invoke();

                var afterInvoke = await GetCountAsync(scope);
                await scope.ExecuteAsync("INSERT INTO test (reference) VALUES ('" + what + "')");
                Assert.AreEqual(afterInvoke + 1, await GetCountAsync(scope));

                return TransactionResult.Rollback;
            });

            Assert.AreEqual(initial, await GetCountAsync(adapter));
        }

        [TestMethod]
        public async Task TestNestedTransactions()
        {
            var connString = new SqliteConnectionStringBuilder { DataSource = Path.Combine(Path.GetTempPath(), "TestNestedTransactions.s3db") };
            var adapter = await new SqliteCommandChannelFactory()
                .CreateAsync(new CreationParameters<SqliteConnectionStringBuilder>(connString, Resources.TestCommitRollback, true));

            await DoSomethingInsertAndCommit(adapter, "B",
                () => DoSomethingInsertAndRollback(adapter, "C",
                    () => InsertDoSomethingAndCommit(adapter, "D",
                        () => InsertDoSomethingAndRollback(adapter, "E", () => Task.CompletedTask)
                    )
                )
            );

            Assert.AreEqual(1, await GetCountAsync(adapter));
        }
    }
}
