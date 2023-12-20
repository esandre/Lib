using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;

namespace Lib.SQL.MySQL.Test;

[TestClass]
public sealed class TestTransaction : TestAbstract
{
    [TestMethod]
    public async Task TestCommit()
    {
        await InsertValueInNewDbAsync(true);

        var adapter = new MySQLCommandChannelFactory()
            .OpenAsync(Credentials);
        Assert.AreEqual("committed", await adapter.FetchValueAsync("SELECT reference FROM test LIMIT 1"));
    }

    [TestMethod]
    public async Task TestRollback()
    {
        await InsertValueInNewDbAsync(false);

        var adapter = new MySQLCommandChannelFactory().OpenAsync(Credentials);
        Assert.AreEqual((long) 0, await adapter.FetchValueAsync("SELECT COUNT(*) FROM test"));
    }

    private async Task InsertValueInNewDbAsync(bool commit)
    {
        var adapter = await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCommitRollback, true));

        await adapter.ExecuteInTransactionAsync(async scope =>
        {
            await scope.ExecuteAsync("INSERT INTO test (reference) VALUES (@value)", new Dictionary<string, IConvertible>{{"@value", "committed" } });
            return commit ? TransactionResult.Commit : TransactionResult.Rollback;
        });
    }

    private static async Task<int> GetCountAsync(ITable table, IAsyncCommandChannel adapter)
    {
        return Convert.ToInt32(await table.Select("COUNT(*)").ExecuteOnAsync(adapter));
    }

    private static async Task InsertDoSomethingAndCommitAsync(IAsyncCommandChannel commandChannel, ITable table, string what, Func<Task> something)
    {
        var beforeCommit = 0;
        await commandChannel.ExecuteInTransactionAsync(async scope =>
        {
            var initial = await GetCountAsync(table, scope);
            await table.Insert().Values(what).ExecuteOnAsync(scope);
            Assert.AreEqual(initial + 1, await GetCountAsync(table, scope));

            await something.Invoke();

            beforeCommit = await GetCountAsync(table, scope);
            return TransactionResult.Commit;
        });
        Assert.AreEqual(beforeCommit, await GetCountAsync(table, commandChannel));
    }

    private static async Task InsertDoSomethingAndRollbackAsync(IAsyncCommandChannel commandChannel, ITable table, string what, Func<Task> something)
    {
        var initial = await GetCountAsync(table, commandChannel);
        await commandChannel.ExecuteInTransactionAsync( async scope =>
        {
            await table.Insert().Values(what).ExecuteOnAsync(scope);
            Assert.AreEqual(initial + 1, await GetCountAsync(table, scope));
            await something.Invoke();
            return TransactionResult.Rollback;
        });
        Assert.AreEqual(initial, await GetCountAsync(table, commandChannel));
    }

    private static async Task DoSomethingInsertAndCommitAsync(IAsyncCommandChannel commandChannel, Table table, string what, Func<Task> something)
    {
        var initial = await GetCountAsync(table, commandChannel);

        await commandChannel.ExecuteInTransactionAsync(async scope =>
        {
            await something.Invoke();
            await table.Insert().Values(what).ExecuteOnAsync(scope);
            return TransactionResult.Commit;
        });

        Assert.AreEqual(initial + 1, await GetCountAsync(table, commandChannel));
    }

    private static async Task DoSomethingInsertAndRollbackAsync(IAsyncCommandChannel commandChannel, Table table, string what, Func<Task> something)
    {
        var initial = await GetCountAsync(table, commandChannel);

        await commandChannel.ExecuteInTransactionAsync(async scope =>
        {
            await something.Invoke();

            var afterInvoke = await GetCountAsync(table, scope);
            await table.Insert().Values(what).ExecuteOnAsync(scope);
            Assert.AreEqual(afterInvoke + 1, await GetCountAsync(table, scope));

            return TransactionResult.Rollback;
        });

        Assert.AreEqual(initial, await GetCountAsync(table, commandChannel));
    }

    [TestMethod]
    public async Task TestNestedTransactions()
    {
        var adapter = await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCommitRollback, true));
        var table = new Table("test", "reference");

        await DoSomethingInsertAndCommitAsync(adapter, table, "B",
            () => DoSomethingInsertAndRollbackAsync(adapter, table, "C",
                () => InsertDoSomethingAndCommitAsync(adapter, table, "D",
                    () => InsertDoSomethingAndRollbackAsync(adapter, table, "E", () => Task.CompletedTask)
                )
            )
        );

        Assert.AreEqual(1, await GetCountAsync(table, adapter));
    }
}