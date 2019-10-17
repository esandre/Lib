using System;
using System.Linq;
using Lib.SQL.QueryBuilder.Params;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.QueryBuilder
{
    [TestClass]
    public class TestParamsCollection
    {
        [TestMethod]
        public void TestCollection()
        {
            var collection = new ParamsCollection();

            Assert.AreEqual("@" + Math.Abs("test".GetHashCode()), collection.GetIdentifier("test"));
            Assert.AreEqual(1, collection.Params.Count);
            Assert.AreEqual("@" + Math.Abs("test".GetHashCode()), collection.Params.First().Key);
            Assert.AreEqual("test", collection.Params.First().Value);
        }

        [TestMethod]
        public void TestNull()
        {
            var collection = new ParamsCollection();
            collection.GetIdentifier(null);
            Assert.AreEqual(0, collection.Params.Count);
        }
    }
}
