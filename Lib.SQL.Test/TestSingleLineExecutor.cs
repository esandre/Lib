using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleLineExecutor
    {
        [TestMethod]
        public void TestSingleLine()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLine(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) ==
                new Dictionary<string, IConvertible> {{"a", "b"}, {"c", "d"}}
            );

            var result = new SingleLineExecutor().ExecuteOnAdapter(commandChannel, "");
            Assert.AreEqual("b", result["a"].ToString());
            Assert.AreEqual("d", result["c"].ToString());
        }

        [TestMethod]
        public void TestExceptionsNotSwallowed()
        {
            var executor = new SingleLineExecutor();

            var commandChannel = new Mock<ICommandChannel>();
            commandChannel
                .Setup(m => m.FetchLine(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            Assert.ThrowsException<VerySpecificException>(() => executor.ExecuteOnAdapter(commandChannel.Object, ""));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new SingleLineExecutor();

            var commandChannel = new Mock<IAsyncCommandChannel>();
            commandChannel
                .Setup(m => m.FetchLineAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            await Assert.ThrowsExceptionAsync<VerySpecificException>(async () => await executor.ExecuteOnAdapterAsync(commandChannel.Object, ""));
        }

        private class VerySpecificException : Exception
        {
        }
    }
}
