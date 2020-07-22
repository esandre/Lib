using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    /// <summary>
    /// A Transceiver which manages the underlying serial port open/close.
    /// </summary>
    public class PortAutomaticOpenCloseTransceiver : ISerialPortTransceiver
    {
        private readonly PortAlreadyOpenBeforeBehavior _alreadyOpenBeforeBehavior;
        private readonly ISerialPortTransceiver _inner;

        public PortAutomaticOpenCloseTransceiver(ISerialPortTransceiver inner, PortAlreadyOpenBeforeBehavior alreadyOpenBeforeBehavior)
        {
            _alreadyOpenBeforeBehavior = alreadyOpenBeforeBehavior;
            _inner = inner;
        }

        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken)
        {
            var wasOpenBefore = _inner.IsOpen;

            if(!wasOpenBefore) _inner.Open();
            var result = await _inner.TransceiveAsync(request, cancellationToken);
            if(PortMustBeClosed(wasOpenBefore)) _inner.Close();

            return result;
        }

        private bool PortMustBeClosed(bool itWasOpenBefore) =>
            !itWasOpenBefore || _alreadyOpenBeforeBehavior == PortAlreadyOpenBeforeBehavior.CloseIt;

        public enum PortAlreadyOpenBeforeBehavior
        {
            LeaveItOpen,
            CloseIt
        }

        public void Open() => _inner.Open();
        public void Close() => _inner.Close();
        public bool IsOpen => _inner.IsOpen;

        public void Dispose()
        {
            _inner?.Dispose();
        }
    }
}
