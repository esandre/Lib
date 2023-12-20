using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;

namespace Lib.SQL.MySQL.Test;

[TestClass]
public sealed class TestConnection : TestAbstract
{
    [TestMethod]
    public async Task TestCreationSuccessFromPlainSql()
    {
        await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCreationSuccess, true));
    }

    [TestMethod]
    [ExpectedException(typeof(Exception), AllowDerivedTypes = true)]
    public async Task TestCreationFailFromFromPlainSql()
    {
        await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, Resources.TestCreationFail, true));
    }

    [TestMethod]
    public async Task TestOpeningSuccess()
    {
        await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "", true));
    }

    [TestMethod]
    public async Task TestSuccessiveConnections()
    {
        var adapter = await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));
        
        await adapter.ExecuteAsync("INSERT INTO a VALUES ('c')");
        
        adapter = await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));
        
        Assert.AreEqual(0, (await adapter.FetchLinesAsync("SELECT * FROM a")).Count);
    }

    [TestMethod]
    public async Task TestMultithreading()
    {
        var mainAdapter = await new MySQLCommandChannelFactory()
            .CreateAsync(new CreationParameters<MySqlConnectionStringBuilder>(Credentials, "CREATE TABLE a (b TEXT)", true));

        var bag = new ConcurrentBag<Task>();

        var t1 = new Thread(() =>
        {
            var adapter =  new MySQLCommandChannelFactory().OpenAsync(Credentials);
            foreach (var _ in Enumerable.Repeat(0, 50)) bag.Add(adapter.ExecuteAsync("INSERT INTO a VALUES ('c')"));
        });

        var t2 = new Thread(() =>
        {
            var adapter = new MySQLCommandChannelFactory().OpenAsync(Credentials);
            foreach (var _ in Enumerable.Repeat(0, 50)) bag.Add(adapter.ExecuteAsync("INSERT INTO a VALUES ('c')"));
        });

        var t3 = new Thread(() =>
        {
            var adapter = new MySQLCommandChannelFactory().OpenAsync(Credentials);
            foreach (var _ in Enumerable.Repeat(0, 50)) bag.Add(adapter.ExecuteAsync("INSERT INTO a VALUES ('c')"));
        });

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        var tasks = bag.ToArray();
        await Task.WhenAll(tasks);

        Assert.AreEqual(150, (await mainAdapter.FetchLinesAsync("SELECT * FROM a")).Count);
    }
}