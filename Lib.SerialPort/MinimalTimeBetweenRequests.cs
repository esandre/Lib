using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public class MinimalTimeBetweenRequests : ISerialPortTransceiver
    {
        private readonly ISerialPortTransceiver _serialPortTransceiverImplementation;
        private readonly TimeSpan _minimalTime;
        private DateTime _lastRequestStamp;

        public MinimalTimeBetweenRequests(ISerialPortTransceiver serialPortTransceiverImplementation, TimeSpan minimalTime)
        {
            _serialPortTransceiverImplementation = serialPortTransceiverImplementation;
            _minimalTime = minimalTime;
            _lastRequestStamp = DateTime.MinValue;
        }

        public void Open()
        {
            _serialPortTransceiverImplementation.Open();
        }

        public void Close()
        {
            _serialPortTransceiverImplementation.Close();
        }

        public bool IsOpen => _serialPortTransceiverImplementation.IsOpen;

        public void Dispose()
        {
            _serialPortTransceiverImplementation.Dispose();
        }

        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken)
        {
            var differenceWithStamp = _lastRequestStamp + _minimalTime - DateTime.Now;
            if (differenceWithStamp > TimeSpan.Zero)
            {
                await Task.Delay(differenceWithStamp, cancellationToken);
            }

            try
            {
                return await _serialPortTransceiverImplementation.TransceiveAsync(request, cancellationToken);
            }
            finally
            {
                _lastRequestStamp = DateTime.Now;
            }
        }
    }
}
