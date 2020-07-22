using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.SerialPort.Exceptions;

namespace Lib.SerialPort
{
    internal class SerialPortReceiver
    {
        private readonly ISerialPort _serialPort;
        private readonly byte _packetBeginning;
        private readonly byte _packetEnd;

        public SerialPortReceiver(ISerialPort serialPort, byte packetBeginning, byte packetEnd)
        {
            _serialPort = serialPort;
            _packetBeginning = packetBeginning;
            _packetEnd = packetEnd;
        }

        /// <remarks>The port must be open before the call</remarks>
        public async Task<byte[]> ReadAsync(CancellationToken cancellationToken)
        {
            var received = new List<byte>();
            var captureBegan = false;
            var captureEnd = false;

            try
            {
                // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                while (!captureEnd)
                    await Task.Run(() =>
                    {
                        var nextByte = _serialPort.ReadByte();

                        if (nextByte == -1)
                        {
                            captureEnd = true;
                            return;
                        }
                        if (nextByte == _packetBeginning) captureBegan = true;
                        if (nextByte == _packetEnd && captureBegan) captureEnd = true;

                        received.Add(Convert.ToByte(nextByte));
                    }, cancellationToken);

                return received.ToArray();
            }
            catch (Exception e)
            {
                throw new ReceptionException(e, received.ToArray());
            }
            finally
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
            }
        }
    }
}
