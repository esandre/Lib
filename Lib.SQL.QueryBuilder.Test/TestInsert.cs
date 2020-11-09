using System;
using Lib.SQL.QueryBuilder.Sequences;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.QueryBuilder.Test
{
    [TestClass]
    public class TestInsert
    {
        [TestMethod]
        public void TestSimpleInsert()
        {
            var insert = Insert.Into("test", "a", "c").Values("b", "d");
            Assert.AreEqual(string.Format("INSERT INTO test (a, c) VALUES (@{0}, @{1})", Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode())), insert.Sql);
        }

        [TestMethod]
        public void TestNullInsert()
        {
            var insert = Insert.Into("test", "a", "c").Values("b", null);
            Assert.AreEqual(string.Format("INSERT INTO test (a, c) VALUES (@{0}, NULL)", Math.Abs("b".GetHashCode())), insert.Sql);
        }

        [TestMethod]
        public void TestOnError()
        {
            var insert = Insert.Into("test", "a", "c").Values("b", "d").OnError(OrType.Replace);
            Assert.AreEqual(string.Format("INSERT OR REPLACE INTO test (a, c) VALUES (@{0}, @{1})", Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode())), insert.Sql);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoLineInsert()
        {
            var insert = Insert.Into("test", "a", "c");
            Assert.IsNotNull(insert.Sql);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoColumnInsert()
        {
            var insert = Insert.Into("test");
            Assert.IsNotNull(insert.Sql);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestBadBatchInsert()
        {
            var insert = Insert.Into("test", "a", "c").Values("b", "d", "o");
            Assert.IsNotNull(insert.Sql);
        }

        [TestMethod]
        public void TestTwoBatchesInsert()
        {
            var insert = Insert.Into("test", "a", "c").Values("b", "d").Values("e", "f");
            Assert.AreEqual(string.Format("INSERT INTO test (a, c) VALUES (@{0}, @{1}), (@{2}, @{3})",
                Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode()), Math.Abs("e".GetHashCode()), Math.Abs("f".GetHashCode())), insert.Sql);
        }
    }
}
