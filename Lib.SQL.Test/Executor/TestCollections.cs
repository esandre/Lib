using System.Collections.Generic;
using Lib.SQL.Executor.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestCollections
    {
        [TestMethod]
        public void TestResultLine()
        {
            var line = new ResultLine(new Dictionary<string, object> {{"aaa", "bbb"}});
            Assert.AreEqual("bbb", line["aaa"]);
        }

        [TestMethod]
        public void TestResultLines()
        {
            var lines = new ResultLines(new List<IDictionary<string, object>> {new Dictionary<string, object> {{"aaa", "bbb"}}});
            Assert.AreEqual("bbb", lines[0]["aaa"]);
        }

        [TestMethod]
        public void TestEmptyResultLine()
        {
            var line = new ResultLine(new Dictionary<string, object>());
            Assert.AreEqual(0, line.Count);
        }

        [TestMethod]
        public void TestEmptyResultLines()
        {
            var lines = new ResultLines(new List<IDictionary<string, object>>());
            Assert.AreEqual(0, lines.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestNullResultLine()
        {
            var line = new ResultLine(null);
            Assert.AreEqual(0, line.Count);
        }

        [TestMethod]
        public void TestNullResultLines()
        {
            var lines = new ResultLines(null);
            Assert.AreEqual(0, lines.Count);
        }
    }
}
