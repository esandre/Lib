using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.SerialPort
{
    public class MaxResponseTimeTransceiver : ISerialPortTransceiver
    {
        private readonly TimeSpan _maxResponseTime;
        private readonly ISerialPortTransceiver _transceiver;

        public MaxResponseTimeTransceiver(TimeSpan maxResponseTime, ISerialPortTransceiver transceiver)
        {
            _maxResponseTime = maxResponseTime;
            _transceiver = transceiver;
        }

        public void Open() => _transceiver.Open();
        public void Close() => _transceiver.Close();
        public bool IsOpen => _transceiver.IsOpen;
        public void Dispose() => _transceiver.Dispose();

        public async Task<IEnumerable<byte>> TransceiveAsync(byte[] request, CancellationToken cancellationToken)
        {
            var transceivingTaskTokenSource = new CancellationTokenSource();
            cancellationToken.Register(transceivingTaskTokenSource.Cancel);

            var cancelTask = Task
                .Delay(_maxResponseTime, transceivingTaskTokenSource.Token)
                .ContinueWith(_ => Enumerable.Empty<byte>(), transceivingTaskTokenSource.Token);

            var transceivingTask = _transceiver.TransceiveAsync(request, transceivingTaskTokenSource.Token);

            var completed = await Task.WhenAny(cancelTask, transceivingTask);

            if (completed == cancelTask && !transceivingTask.IsCompleted) 
                transceivingTaskTokenSource.Cancel();

            return await completed;
        }
    }
}
