namespace Lib.SerialPort
{
    public class RealSerialPort : ISerialPort
    {
        private readonly System.IO.Ports.SerialPort _port;

        public RealSerialPort(System.IO.Ports.SerialPort port)
        {
            _port = port;
        }

        public int BytesToRead => _port.BytesToRead;
        public int ReadByte() => _port.ReadByte();
        public void Open() => _port.Open();
        public void DiscardOutBuffer() => _port.DiscardOutBuffer();
        public void DiscardInBuffer() => _port.DiscardInBuffer();
        public void Write(byte[] request, int i, in int requestLength) => _port.Write(request, i, requestLength);
        public void Close()
        {
            try
            {
                _port.Close();
            }
            catch
            {
            }
        }

        public void Dispose() => _port.Dispose();
        public bool IsOpen => _port.IsOpen;

        public override string ToString()
        {
            return _port.PortName;
        }
    }
}
