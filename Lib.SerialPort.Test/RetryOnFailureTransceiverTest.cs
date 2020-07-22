using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.SerialPort.Test
{
    [TestClass]
    public class RetryOnFailureTransceiverTest
    {
        [TestMethod]
        public async Task Si_Le_Transceiver_Renvoie_Une_Exception_Retente()
        {
            Expression<Func<ISerialPortTransceiver, Task<IEnumerable<byte>>>> anyCallToTransceiveAsync =
                m => m.TransceiveAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>());

            var faultedTransceiver = new Mock<ISerialPortTransceiver>();
            faultedTransceiver
                .SetupSequence(anyCallToTransceiveAsync)
                .Throws(new Exception())
                .Returns(Task.FromResult(Enumerable.Empty<byte>()));

            var retryOnFailure = new RetryOnFailureTrasceiver(faultedTransceiver.Object);
            await retryOnFailure.TransceiveAsync(new byte[0], CancellationToken.None);

            faultedTransceiver.Verify(anyCallToTransceiveAsync, Times.Exactly(2));
        }

        [TestMethod]
        public void Si_Le_Transceiver_Renvoie_Une_Seconde_Exception_Throw_Les_Deux_Imbriquées()
        {
            var firstException = new Exception("First");
            var secondException = new Exception("Second");

            var faultedTransceiver = new Mock<ISerialPortTransceiver>();
            faultedTransceiver
                .SetupSequence(m => m.TransceiveAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
                .Throws(firstException)
                .Throws(secondException)
                .Returns(Task.FromResult(Enumerable.Empty<byte>()));

            var retryOnFailure = new RetryOnFailureTrasceiver(faultedTransceiver.Object);

            Check.ThatAsyncCode((Func<Task>) (async () => await retryOnFailure.TransceiveAsync(new byte[0], CancellationToken.None)))
                .Throws<AggregateException>()
                .WhichMember(m => m.InnerExceptions)
                .ContainsExactly(firstException, secondException);
        }
    }
}
