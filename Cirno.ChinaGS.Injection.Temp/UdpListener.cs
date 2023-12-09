using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Cirno.ChinaGS.Injection.Temp
{
    public class UdpListener : IDisposable
    {
        //public Socket localUdpServer;
        public UdpClient udpClient;
        public IPEndPoint localIPEndPoint;
        public int port;

        public UdpListener(int port)
        {
            this.port = port;
            localIPEndPoint = new IPEndPoint(IPAddress.Any, port);
            //localUdpServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //localUdpServer.Bind(localIPEndPoint);

            udpClient = new UdpClient(localIPEndPoint);
        }

        public void Dispose()
        {
            udpClient.Close();
        }
    }
}
