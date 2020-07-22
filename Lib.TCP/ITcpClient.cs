using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Lib.TCP
{
    public interface ITcpClient : IDisposable
    {
        void Close();
        Task ConnectAsync(IPAddress address, int port);
        Stream GetStream();
    }
}
