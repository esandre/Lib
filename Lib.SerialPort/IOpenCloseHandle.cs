namespace Lib.SerialPort
{
    public interface IOpenCloseHandle
    {
        void Open();
        void Close();
        bool IsOpen { get; }
    }
}
