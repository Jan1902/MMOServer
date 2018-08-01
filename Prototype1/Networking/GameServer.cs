using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prototype1
{
    /// <summary>
    /// The Main Game Server class, responsible for storing important data and connection management
    /// </summary>
    class GameServer
    {
        private Host _host;
        private bool _stopRequested;

        public List<ClientConnectionInfo> Connections { get; private set; }

        /// <summary>
        /// The singleton instance
        /// </summary>
        public static readonly GameServer Instance = new GameServer();

        public PacketHandlerManager PacketHandlerManager { get; private set; }

        /// <summary>
        /// Initializes the GameServer
        /// </summary>
        public void Init()
        {
            PacketHandlerManager = new PacketHandlerManager();
            Connections = new List<ClientConnectionInfo>();

            PacketHandlerManager.SetupHandlers();

            _host = new Host();
            _host.InitializeServer(Constants.Port, Constants.MaxPlayers);
            NetLoop();
        }
        /// <summary>
        /// The loop function, responsible for listening for connections, data, and disconnects
        /// </summary>
        private void NetLoop()
        {
            while (!_stopRequested)
            {
                while (_host.Service(Constants.HostEventTimeout, out Event enetEvent))
                {
                    switch (enetEvent.Type)
                    {
                        case EventType.Connect:
                            Console.WriteLine("Client connected on {0}", enetEvent.Peer.GetRemoteAddress());
                            Connections.Add(new ClientConnectionInfo(enetEvent.Peer));
                            break;

                        case EventType.Receive:
                            PacketHandlerManager.HandleData(enetEvent.Packet.GetBytes(), GetConnectionInfoByPeer(enetEvent.Peer));
                            enetEvent.Packet.Dispose();
                            break;

                        case EventType.Disconnect:
                            Console.WriteLine("Client on {0} disconnected", enetEvent.Peer.GetRemoteAddress());
                            Connections.Remove(GetConnectionInfoByPeer(enetEvent.Peer));
                            break;
                        default:
                            Console.WriteLine("Invalid event called");
                            break;
                    }
                }
                Thread.Sleep(1);
            }
        }

        public ClientConnectionInfo GetConnectionInfoByPeer(Peer peer)
        {
            return Connections.First(con => con.Peer.GetRemoteAddress().Port == peer.GetRemoteAddress().Port);
        }
    }
}
