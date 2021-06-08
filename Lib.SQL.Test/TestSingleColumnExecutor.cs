using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleColumnExecutor
    {
        [TestMethod]
        public void TestSingleColumn()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLines(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) ==
                new[]
                {
                    new Dictionary<string, IConvertible> {{"_", "b"}},
                    new Dictionary<string, IConvertible> {{"_", "f"}},
                }
            );

            var result = new SingleColumnExecutor().ExecuteOnAdapter(commandChannel, "");

            Assert.AreEqual("b", result[0]);
            Assert.AreEqual("f", result[1]);
        }

        [TestMethod]
        public void TestExceptionsNotSwallowed()
        {
            var executor = new SingleColumnExecutor();

            var commandChannel = new Mock<ICommandChannel>();
            commandChannel
                .Setup(m => m.FetchLines(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            Assert.ThrowsException<VerySpecificException>(() => executor.ExecuteOnAdapter(commandChannel.Object, ""));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new SingleColumnExecutor();

            var commandChannel = new Mock<IAsyncCommandChannel>();
            commandChannel
                .Setup(m => m.FetchLinesAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            await Assert.ThrowsExceptionAsync<VerySpecificException>(async () => await executor.ExecuteOnAdapterAsync(commandChannel.Object, ""));
        }

        private class VerySpecificException : Exception
        {
        }
    }
}
