using System;

namespace Lib.SerialPort
{
    public class SerialPortLogger : ISerialPort
    {
        private readonly ISerialPort _logged;
        private readonly IProgress<string> _log;
        private readonly IByteFormatter _byteFormatter;

        public SerialPortLogger(ISerialPort logged, IProgress<string> log, IByteFormatter byteFormatter)
        {
            _logged = logged;
            _log = log;
            _byteFormatter = byteFormatter;
        }

        public void Open()
        {
            _logged.Open();
            _log.Report("Port is now open");
        }

        public void Close()
        {
            _logged.Close();
            _log.Report("Port is now closed");
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
            _log.Report("Received : " + _byteFormatter.FormatReceivedSingleByte(b));
            return b;
        }

        public void DiscardOutBuffer()
        {
            _logged.DiscardOutBuffer();
            _log.Report("Out buffer discarded");
        }

        public void DiscardInBuffer()
        {
            _logged.DiscardInBuffer();
            _log.Report("In buffer discarded");
        }

        public void Write(byte[] request, int i, in int requestLength)
        {
            _logged.Write(request, i, in requestLength);
            _log.Report("Written : " + _byteFormatter.FormatSentByteArray(request));
        }
    }
}
