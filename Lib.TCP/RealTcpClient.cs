using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Lib.TCP
{
    public class RealTcpClient : ITcpClient
    {
        private readonly TcpClient _realClient;

        public RealTcpClient(TcpClient realClient)
        {
            _realClient = realClient;
        }

        public void Dispose() => _realClient.Dispose();
        public void Close() => _realClient.Close();
        public Task ConnectAsync(IPAddress address, int port) => _realClient.ConnectAsync(address, port);
        public Stream GetStream() => _realClient.GetStream();
    }
}
