using Microsoft.Extensions.Logging;

namespace Lib.SerialPort
{
    public class SerialPortLogger : ISerialPort
    {
        private readonly ISerialPort _logged;
        private readonly ILogger<SerialPortLogger> _log;
        private readonly IByteFormatter _byteFormatter;

        public SerialPortLogger(ISerialPort logged, ILogger<SerialPortLogger> log, IByteFormatter byteFormatter)
        {
            _logged = logged;
            _log = log;
            _byteFormatter = byteFormatter;
        }

        public void Open()
        {
            _logged.Open();
            _log.LogTrace("Port is now open");
        }

        public void Close()
        {
            _logged.Close();
            _log.LogTrace("Port is now closed");
        }

        public bool IsOpen => _logged.IsOpen;
        public int BytesToRead => _logged.BytesToRead;
        
        public void Dispose()
        {
            _logged.Dispose();
        }

        public int ReadByte()
        {
            var b = _logged.ReadByte();

            if(_log.IsEnabled(LogLevel.Trace))
                _log.LogTrace("Received : {received}", _byteFormatter.FormatReceivedSingleByte(b));

            return b;
        }

        public void DiscardOutBuffer()
        {
            _logged.DiscardOutBuffer();
            _log.LogTrace("Out buffer discarded");
        }

        public void DiscardInBuffer()
        {
            _logged.DiscardInBuffer();
            _log.LogTrace("In buffer discarded");
        }

        public void Write(byte[] request, int i, in int requestLength)
        {
            _logged.Write(request, i, in requestLength);

            if(_log.IsEnabled(LogLevel.Trace))
                _log.LogTrace("Written : {written}", _byteFormatter.FormatSentByteArray(request));
        }
    }
}
