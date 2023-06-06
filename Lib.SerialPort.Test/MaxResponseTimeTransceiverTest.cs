using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NFluent;

namespace Lib.SerialPort.Test
{
    [TestClass]
    public class MaxResponseTimeTransceiverTest
    {
        private static readonly byte[] EmptyMessage = new byte[0];
        private static readonly byte[] NotEmptyMessage = { 0x01 };

        [TestMethod]
        public async Task Renvoie_Un_Datagramme_Vide_Si_Le_Temps_Est_Ecoulé()
        {
            var maxTime = TimeSpan.FromMilliseconds(100);
            var inner = Mock.Of<ISerialPortTransceiver>(m => 
                m.TransceiveAsync(EmptyMessage, CancellationToken.None) == WaitTimeAndReturn(maxTime * 5)
            );

            var transceiver = new MaxResponseTimeTransceiver(maxTime, inner);
            var received = await transceiver.TransceiveAsync(EmptyMessage, CancellationToken.None);
            Check.That(received).Equals(EmptyMessage);
        }

        private static async Task<IEnumerable<byte>> WaitTimeAndReturn(TimeSpan time)
        {
            await Task.Delay(time);
            return NotEmptyMessage;
        }
    }
}
