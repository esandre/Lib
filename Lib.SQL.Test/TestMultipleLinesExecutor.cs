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
    public class TestMultipleLinesExecutor
    {
        [TestMethod]
        public void TestMultipleLines()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLines(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) ==
                new[]
                {
                    new Dictionary<string, IConvertible> {{"a", "b"}, {"c", "d"}},
                    new Dictionary<string, IConvertible> {{"a", "f"}, {"c", "h"}},
                }
            );

            var result = new MultipleLinesExecutor().ExecuteOnAdapter(commandChannel, "");
            Assert.AreEqual("b", result[0]["a"].ToString());
            Assert.AreEqual("d", result[0]["c"].ToString());
            Assert.AreEqual("f", result[1]["a"].ToString());
            Assert.AreEqual("h", result[1]["c"].ToString());
        }

        [TestMethod]
        public void TestExceptionsNotSwallowed()
        {
            var executor = new MultipleLinesExecutor();

            var commandChannel = new Mock<ICommandChannel>();
            commandChannel
                .Setup(m => m.FetchLines(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            Assert.ThrowsException<VerySpecificException>(() => executor.ExecuteOnAdapter(commandChannel.Object, ""));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new MultipleLinesExecutor();

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
