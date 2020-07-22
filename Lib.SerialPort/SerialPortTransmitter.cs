using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.SerialPort.Exceptions;

namespace Lib.SerialPort
{
    internal class SerialPortTransmitter
    {
        private readonly ISerialPort _serialPort;

        public SerialPortTransmitter(ISerialPort serialPort)
        {
            _serialPort = serialPort;
        }

        private void Write(byte[] bytes)
        {
            try
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                throw new TransmissionException(e);
            }
        }

        /// <remarks>The port must be open before the call</remarks>
        public async Task WriteAsync(byte[] bytes, CancellationToken cancellationToken) 
            => await Task.Run(() => Write(bytes), cancellationToken);
    }
}
