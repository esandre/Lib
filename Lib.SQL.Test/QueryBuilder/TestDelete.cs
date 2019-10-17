using System;
using System.Linq;
using Lib.SQL.QueryBuilder;
using Lib.SQL.QueryBuilder.Operator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
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

            Assert.AreEqual(String.Format("DELETE FROM table WHERE a = @{0} AND c = @{1} OR e = @{2} AND (g = @{3}) OR (i = @{4})",
                Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode()), Math.Abs("f".GetHashCode()), 
                Math.Abs("h".GetHashCode()), Math.Abs("j".GetHashCode())), query.Sql);
            Assert.AreEqual(5, query.Parameters.Count);
        }
    }
}
