using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public class SemaphoreTransceiver : ISerialPortTransceiver
    {
        private readonly ISerialPortTransceiver _inner;
        private readonly Semaphore _semaphore = new Semaphore(1, 1);
        
        public SemaphoreTransceiver(ISerialPortTransceiver inner)
        {
            _inner = inner;
        }

        public void Open()
        {
            _semaphore.WaitOne();
            _inner.Open();
        }

        public void Close()
        {
            _inner.Close();
            _semaphore.Release();
        }

        public bool IsOpen => _inner.IsOpen;

        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken) 
            => await _inner.TransceiveAsync(request, cancellationToken);

        public void Dispose()
        {
            _inner?.Dispose();
            _semaphore?.Dispose();
        }
    }
}
