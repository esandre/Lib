using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestAffectedLinesExecutor
    {
        [TestMethod]
        public async Task TestLastinsertedIdAsync()
        {
            var executor = new AffectedLinesExecutor();

            var commandChannel = Mock.Of<IAsyncCommandChannel>(m => m.ExecuteAsync(
                It.IsAny<string>(), 
                It.IsAny<IDictionary<string, IConvertible>>()) == Task.FromResult(1));
            Assert.AreEqual(1, await executor.ExecuteOnAdapterAsync(commandChannel, ""));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new AffectedLinesExecutor();

            var commandChannel = new Mock<IAsyncCommandChannel>();
            commandChannel
                .Setup(m => m.ExecuteAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            await Assert.ThrowsExceptionAsync<VerySpecificException>(async () => await executor.ExecuteOnAdapterAsync(commandChannel.Object, ""));
        }

        private class VerySpecificException : Exception;
    }
}
