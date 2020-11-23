using System;
using System.Collections.Generic;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleValueExecutor
    {
        [TestMethod]
        public void TestString()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) == (object) "str"
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual("str", executor.ExecuteOnAdapter(commandChannel, "string"));
        }

        [TestMethod]
        public void TestInt()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) == (object) 1
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(1, executor.ExecuteOnAdapter(commandChannel, "int"));
        }

        [TestMethod]
        public void TestDouble()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) == (object) 8.0
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(8.0, executor.ExecuteOnAdapter(commandChannel, "double"));
        }

        [TestMethod]
        public void TestDateTime()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) == (object) DateTime.MinValue
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(DateTime.MinValue, executor.ExecuteOnAdapter(commandChannel, "date"));
        }
    }
}
