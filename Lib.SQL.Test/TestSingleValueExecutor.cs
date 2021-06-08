using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
// ReSharper disable PossibleUnintendedReferenceComparison
#pragma warning disable 252,253

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleValueExecutor
    {
        [TestMethod]
        public void TestString()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                    m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) == "str"
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual("str", executor.ExecuteOnAdapter(commandChannel, "string"));
        }

        [TestMethod]
        public void TestInt()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) == (IConvertible) 1
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(1, executor.ExecuteOnAdapter(commandChannel, "int"));
        }

        [TestMethod]
        public void TestDouble()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) == (IConvertible) 8.0
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(8.0, executor.ExecuteOnAdapter(commandChannel, "double"));
        }

        [TestMethod]
        public void TestDateTime()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchValue(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) == (IConvertible) DateTime.MinValue
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(DateTime.MinValue, executor.ExecuteOnAdapter(commandChannel, "date"));
        }

        [TestMethod]
        public void TestExceptionsNotSwallowed()
        {
            var executor = new SingleValueExecutor();

            var commandChannel = new Mock<ICommandChannel>();
            commandChannel
                .Setup(m => m.FetchValue(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            Assert.ThrowsException<VerySpecificException>(() => executor.ExecuteOnAdapter(commandChannel.Object, ""));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new SingleValueExecutor();

            var commandChannel = new Mock<IAsyncCommandChannel>();
            commandChannel
                .Setup(m => m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            await Assert.ThrowsExceptionAsync<VerySpecificException>(async () => await executor.ExecuteOnAdapterAsync(commandChannel.Object, ""));
        }

        private class VerySpecificException : Exception
        {
        }
    }
}
