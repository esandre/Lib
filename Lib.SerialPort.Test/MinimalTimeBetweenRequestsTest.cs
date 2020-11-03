using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.SerialPort.Test
{
    [TestClass]
    public class MinimalTimeBetweenRequestsTest
    {
        [TestMethod]
        public void Une_Requête_Unique_N_Est_Pas_Retardée()
        {
            var transceiver = Mock.Of<ISerialPortTransceiver>(
                m => m.TransceiveAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()) == Task.FromResult(Enumerable.Empty<byte>())
            );

            var time = TimeSpan.FromSeconds(1);
            var testedTransceiver = new MinimalTimeBetweenRequests(transceiver, time);

            Check.ThatAsyncCode(
                    async () => await testedTransceiver.TransceiveAsync(new byte[0], CancellationToken.None))
                .LastsLessThan(time.TotalMilliseconds, TimeUnit.Milliseconds);
        }

        [TestMethod]
        public void Deux_Requêtes_Sont_Séparées_Par_Le_Temps_Minimal()
        {
            var transceiver = Mock.Of<ISerialPortTransceiver>(
                m => m.TransceiveAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()) == Task.FromResult(Enumerable.Empty<byte>())
            );

            var time = TimeSpan.FromMilliseconds(100);
            var testedTransceiver = new MinimalTimeBetweenRequests(transceiver, time);

            Check.ThatAsyncCode(
                async () =>
                {
                    await testedTransceiver.TransceiveAsync(new byte[0], CancellationToken.None);
                    await testedTransceiver.TransceiveAsync(new byte[0], CancellationToken.None);
                })
                .Not.LastsLessThan(time.TotalMilliseconds, TimeUnit.Milliseconds);
        }
    }
}
