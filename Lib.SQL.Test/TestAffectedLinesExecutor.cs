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
        public void TestLastinsertedId()
        {
            var executor = new AffectedLinesExecutor();

            var commandChannel = Mock.Of<ICommandChannel>(m => m.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()) == 1);
            Assert.AreEqual(1, executor.ExecuteOnAdapter(commandChannel, ""));
        }

        [TestMethod]
        public void TestExceptionsNotSwallowed()
        {
            var executor = new AffectedLinesExecutor();

            var commandChannel = new Mock<ICommandChannel>();
            commandChannel
                .Setup(m => m.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();
            
            Assert.ThrowsException<VerySpecificException>(() => executor.ExecuteOnAdapter(commandChannel.Object, ""));
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

        private class VerySpecificException : Exception
        {
        }
    }
}
