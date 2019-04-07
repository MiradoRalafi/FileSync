using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSync.ServerObjects
{
    class FTPServer
    {
        private TcpListener _listener;

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 92);
            _listener.Start();
            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
        }

        public void Stop()
        {
            if (_listener != null)
                _listener.Stop();
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            TcpClient client = _listener.EndAcceptTcpClient(result);
            ClientConnection connection = new ClientConnection(client);
            ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
        }
    }
}
