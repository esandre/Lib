using System;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestSingleValueExecutor : TestExecutorAbstractAbstract<SingleValueExecutor, object>
    {
        [TestMethod]
        public void TestString()
        {
            var executor = new SingleValueExecutor {Adapter = new TestAdapter()};
            Assert.AreEqual("str", executor.Execute("string"));
        }

        [TestMethod]
        public void TestInt()
        {
            var executor = new SingleValueExecutor { Adapter = new TestAdapter() };
            Assert.AreEqual(1, executor.Execute("int"));
        }

        [TestMethod]
        public void TestDouble()
        {
            var executor = new SingleValueExecutor { Adapter = new TestAdapter() };
            Assert.AreEqual(8.0, executor.Execute("double"));
        }

        [TestMethod]
        public void TestDateTime()
        {
            var executor = new SingleValueExecutor { Adapter = new TestAdapter() };
            Assert.AreEqual(DateTime.MinValue, executor.Execute("date"));
        }
    }
}
