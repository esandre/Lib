using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public interface ISerialPortTransceiver : IOpenCloseHandle, IDisposable
    {
        Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken);
    }
}
