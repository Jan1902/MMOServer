using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer
{
    class ConnectionListener
    {
        private TcpListener _listener;
        public static Client[] Clients = new Client[Constants.MaxPlayers];

        public ConnectionListener()
        {
                _listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 19022));
                _listener.Start();
                _listener.BeginAcceptTcpClient(AcceptCallback, null); //Start Async accepting connections
        }


        public void AcceptCallback(IAsyncResult ar)
        {
            TcpClient client = _listener.EndAcceptTcpClient(ar); //Client connected, so end acceptTCPClient
            _listener.BeginAcceptTcpClient(AcceptCallback, null); //Start Async accepting new connections again.
            client.NoDelay = false;

            // List of clients 
            // Set client data
            //Console.WriteLine("Connection from {0} ", ip);
            return;
           
        }

        public void Stop()
        {
            ConsoleUtils.Warning("Stopping listener");
            _listener.Stop();

        }
    }
}