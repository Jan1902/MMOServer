using ENet;
using MMOServer.ConsoleStuff;
using MMOServer.Game;
using MMOServer.Other;
using MMOServer.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MMOServer.Networking
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
        public ConsoleManager ConsoleManager { get; private set; }

        //maybe move this somewhere else at some point
        private int _nextEntityId;
        public int NextEntityID {
            get
            {
                _nextEntityId++;
                return _nextEntityId;
            }
        }

        public List<World> Worlds { get; private set; }

        private void Init()
        {
            Connections = new List<ClientConnectionInfo>();
            Logger.Initialize();

            PacketHandlerManager = new PacketHandlerManager(this);
            PacketSenderManager = new PacketSenderManager(this);
            ConsoleManager = new ConsoleManager(this);

            Worlds = new List<World>
            {
                new World(0, "Castle World", this) //Just for testing
            };
            ConsoleUtils.Info("Running {0} world instances", Worlds.Count);

            _host = new Host();
            _host.InitializeServer(Constants.Port, Constants.MaxPlayers);
            ConsoleUtils.Info("Successfully set up and running");
        }

        public GameServer()
        {
            Init();
            NetLoop();
        }

        public void Shutdown()
        {
            ConsoleUtils.Warning("Game Server shutting down");

            _stopRequested = true;
            _host.Dispose();
            //disconnect all clients

            ConsoleUtils.Info("Press enter to close");
            Console.ReadLine();
        }

        /// <summary>
        /// The NetLoop function, responsible for listening for connections, data, and disconnects
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
                            ConsoleUtils.Info("Client connected on {0}", enetEvent.Peer.GetRemoteAddress());
                            Connections.Add(new ClientConnectionInfo(enetEvent.Peer));
                            break;

                        case EventType.Receive:
                            PacketHandlerManager.HandleData(enetEvent.Packet.GetBytes(), GetConnectionInfoByPeer(enetEvent.Peer));
                            enetEvent.Packet.Dispose();
                            break;

                        case EventType.Disconnect:
                            ConsoleUtils.Info("Client on {0} disconnected", enetEvent.Peer.GetRemoteAddress());
                            Connections.Remove(GetConnectionInfoByPeer(enetEvent.Peer));
                            break;

                        default:
                            ConsoleUtils.Warning("Invalid event called");
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
