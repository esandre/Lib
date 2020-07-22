using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public class RetryOnFailureTrasceiver : ISerialPortTransceiver
    {
        private readonly ISerialPortTransceiver _serialPortTransceiverImplementation;

        public RetryOnFailureTrasceiver(ISerialPortTransceiver serialPortTransceiverImplementation)
        {
            _serialPortTransceiverImplementation = serialPortTransceiverImplementation;
        }

        public void Open() => _serialPortTransceiverImplementation.Open();

        public void Close() => _serialPortTransceiverImplementation.Close();

        public bool IsOpen => _serialPortTransceiverImplementation.IsOpen;

        public void Dispose() => _serialPortTransceiverImplementation.Dispose();

        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken)
        {
            try
            {
                return await _serialPortTransceiverImplementation.TransceiveAsync(request, cancellationToken);
            }
            catch (Exception first)
            {
                try
                {
                    return await _serialPortTransceiverImplementation.TransceiveAsync(request, cancellationToken);
                }
                catch (Exception second)
                {
                    throw new AggregateException(first, second);
                }
            }
        }
    }
}
