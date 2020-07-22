using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public class SerialPortTransceiver : ISerialPortTransceiver
    {
        private readonly ISerialPort _serialPort;
        private readonly SerialPortReceiver _receiver;
        private readonly SerialPortTransmitter _transmitter;

        public SerialPortTransceiver(ISerialPort serialPort, byte packetBeginning, byte packetEnd)
        {
            _serialPort = serialPort;
            _transmitter = new SerialPortTransmitter(serialPort);
            _receiver = new SerialPortReceiver(serialPort, packetBeginning, packetEnd);
        }

        /// <remarks>The port must be open before the call</remarks>
        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken)
        {
            await _transmitter.WriteAsync(request, cancellationToken);
            return await _receiver.ReadAsync(cancellationToken);
        }

        public void Open() => _serialPort.Open();
        public void Close() => _serialPort.Close();
        public bool IsOpen => _serialPort.IsOpen;

        public void Dispose()
        {
            _serialPort?.Dispose();
        }
    }
}
