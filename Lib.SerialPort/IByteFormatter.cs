namespace Lib.SerialPort
{
    public interface IByteFormatter
    {
        string FormatReceivedSingleByte(int b);
        string FormatSentByteArray(byte[] b);
    }
}
