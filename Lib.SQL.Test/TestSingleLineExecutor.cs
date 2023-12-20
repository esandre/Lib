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
        public async Task TestSingleLine()
        {
            var commandChannel = Mock.Of<IAsyncCommandChannel>(m =>
                m.FetchLineAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) ==
                Task.FromResult<IReadOnlyDictionary<string, IConvertible>>(new Dictionary<string, IConvertible> {{"a", "b"}, {"c", "d"}})
            );

            var result = await new SingleLineExecutor().ExecuteOnAdapterAsync(commandChannel, "");
            Assert.AreEqual("b", result["a"].ToString());
            Assert.AreEqual("d", result["c"].ToString());
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

        private class VerySpecificException : Exception;
    }
}
