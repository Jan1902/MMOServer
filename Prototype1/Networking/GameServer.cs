using ENet;
using Prototype1.Game;
using Prototype1.Other;
using Prototype1.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prototype1.Networking
{
    /// <summary>
    /// The Main Game Server class, responsible for storing important data and connection management
    /// </summary>
    class GameServer
    {
        private Host _host;
        private bool _stopRequested;

        public List<ClientConnectionInfo> Connections { get; private set; }

        public PacketHandlerManager PacketHandlerManager { get; private set; }
        public PacketSenderManager PacketSenderManager { get; private set; }

        //maybe move this somewhere else at some point
        private int _nextEntityId = 0;
        public int NextEntityID { get
            {
                _nextEntityId++;
                return _nextEntityId;
            }
        }

        public List<World> Worlds { get; private set; }

        private void Init()
        {
            PacketHandlerManager = new PacketHandlerManager(this);
            PacketSenderManager = new PacketSenderManager(this);
            Connections = new List<ClientConnectionInfo>();

            Worlds = new List<World>();
            Worlds.Add(new World(0, "Castle World", this));

            _host = new Host();
            _host.InitializeServer(Constants.Port, Constants.MaxPlayers);

        }

        public GameServer()
        {
            Init();
            NetLoop();
        }

        private void Shutdown()
        {
            _host.Dispose();
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
