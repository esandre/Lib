using System;

namespace Lib.SerialPort
{
    public interface ISerialPort : IOpenCloseHandle, IDisposable
    {
        int BytesToRead { get; }
        int ReadByte();
        void DiscardOutBuffer();
        void DiscardInBuffer();
        void Write(byte[] request, int i, in int requestLength);
    }
}
