using System;
using System.Linq;
using Lib.SQL.QueryBuilder.Operator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.QueryBuilder.Test
{
    [TestClass]
    public class TestDelete
    {
        [TestMethod]
        public void TestDeleteAll()
        {
            var query = Delete.From("table");
            Assert.AreEqual("DELETE FROM table", query.Sql);
            Assert.IsFalse(query.Parameters.Any());
        }

        [TestMethod]
        public void TestDeleteWhere()
        {
            var eq = Is.EqualWith;

            var query = Delete.From("table")
                .Where("a", eq, "b").And("c", eq, "d").Or("e", eq, "f").And(
                    sub => sub.Where("g", eq, "h")
                ).Or(
                    sub => sub.Where("i", eq, "j")
                );

            Assert.AreEqual(
                $"DELETE FROM table WHERE a = @{Math.Abs("b".GetHashCode())} AND c = @{Math.Abs("d".GetHashCode())} OR e = @{Math.Abs("f".GetHashCode())} AND (g = @{Math.Abs("h".GetHashCode())}) OR (i = @{Math.Abs("j".GetHashCode())})", query.Sql);
            Assert.AreEqual(5, query.Parameters.Count);
        }
    }
}
