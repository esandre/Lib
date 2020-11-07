using System;
using Lib.SQL.QueryBuilder.Operator;
using Lib.SQL.QueryBuilder.Params;
using Lib.SQL.QueryBuilder.Sequences.Where;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
{
    [TestClass]
    public class TestWhereSequence
    {
        [TestMethod]
        public void TestSimpleWhere()
        {
            var paramsCollection = new ParamsCollection();
            var where = new WhereSequence("key", Is.EqualWith, "val", paramsCollection);
            Assert.AreEqual("key = @" + Math.Abs("val".GetHashCode()), where.ToString());
        }

        [TestMethod]
        public void TestSimpleAndOr()
        {
            var paramsCollection = new ParamsCollection();
            var where = new WhereSequence("a", Is.EqualWith, "b", paramsCollection).Or("c", Is.EqualWith, "d").And("e", Is.EqualWith, "f");
            Assert.AreEqual(string.Format("a = @{0} OR c = @{1} AND e = @{2}",
                Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode()), Math.Abs("f".GetHashCode())), where.ToString());
        }

        [TestMethod]
        public void TestSubsequence()
        {
            var paramsCollection = new ParamsCollection();
            var where = new WhereSequence("a", Is.EqualWith, "b", paramsCollection).And(
                subsequence => subsequence.Where("c", Is.EqualWith, "d").Or("e", Is.EqualWith, "f")
            );
            Assert.AreEqual(string.Format("a = @{0} AND (c = @{1} OR e = @{2})",
                Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode()), Math.Abs("f".GetHashCode())), where.ToString());
        }

        [TestMethod]
        public void TestComplex()
        {
            var paramsCollection = new ParamsCollection();
            var where = new WhereSequence("a", Is.EqualWith, "b", paramsCollection).And(
                subsequence => subsequence.Where("c", Is.EqualWith, "d").Or("e", Is.EqualWith, "f").Or(
                    secondLevel => secondLevel.Where("g", Is.EqualWith, "h").And("i", Is.EqualWith, "j")
                )
            ).Or("k", Is.EqualWith, "l");

            Assert.AreEqual(string.Format("a = @{0} AND (c = @{1} OR e = @{2} OR (g = @{3} AND i = @{4})) OR k = @{5}",
                Math.Abs("b".GetHashCode()), Math.Abs("d".GetHashCode()), Math.Abs("f".GetHashCode()),
                Math.Abs("h".GetHashCode()), Math.Abs("j".GetHashCode()), Math.Abs("l".GetHashCode())), where.ToString());
        }
    }
}
