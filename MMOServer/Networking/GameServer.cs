using ENet;
using MMOServer.Config;
using MMOServer.ConsoleStuff;
using MMOServer.Database;
using MMOServer.Encryption;
using MMOServer.EventBusSystem;
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
    /// The Main Game Server class, responsible for storing important data and network management
    /// </summary>
    class GameServer
    {
        private Host _host;

        private bool _stopRequested;

        public List<ClientConnectionInfo> Connections { get; private set; }

        public PacketHandlerManager PacketHandlerManager { get; private set; }
        public PacketSenderManager PacketSenderManager { get; private set; }

        public ConsoleManager ConsoleManager { get; private set; }
        public ConfigManager ConfigManager { get; private set; }
        public DatabaseManager DatabaseManager { get; private set; }
        public EncryptionManager EncryptionManager { get; private set; }

        public EventBus EventBus { get; private set; }

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
            ConfigManager = new ConfigManager();
            DatabaseManager = new DatabaseManager(this);
            EncryptionManager = new EncryptionManager();

            Worlds = new List<World>
            {
                new World(0, "Castle World", this) //Just for testing
            };
            ConsoleUtils.Info("Running {0} world instances", Worlds.Count);

            EventBus = new EventBus(this, Worlds.Select(w => w.EntityManager).ToList<IGameManager>()); //Only the entity managers, for now

            _host = new Host();
            _host.InitializeServer(ConfigManager.Settings.Port, ConfigManager.Settings.MaxPlayers);
            ConsoleUtils.Info("Successfully set up and running");
        }

        public GameServer()
        {
            Init();
            new Thread(() => NetLoop()).Start();
            new Thread(() => PhysicsLoop()).Start();
            new Thread(() => GameEventLoop()).Start();
        }

        public void Shutdown()
        {
            ConsoleUtils.Warning("Game Server shutting down");

            _stopRequested = true;
            ConfigManager.Save();
            DatabaseManager.Disconnect();
            DisconnectAllClients();

            ConsoleUtils.Info("Press enter to close");
            Console.ReadLine();
        }

        private void DisconnectAllClients()
        {
            ConsoleUtils.Info("Disconnecting all clients");
            foreach(var connection in Connections)
            {
                connection.Disconnect();
            }
        }

        /// <summary>
        /// The PhysicsLoop function, responsible for calling all physics and movement calculations
        /// </summary>
        private void PhysicsLoop()
        {
            while(!_stopRequested)
            {
                //TODO: DO MOVEMENT CALCULATIONS HERE
                Thread.Sleep(1000 / ConfigManager.Settings.PhysicsUpdatesPerSecond);
            }
        }


        /// <summary>
        /// The GameEventLoop function, responsible for calling all physics and movement calculations
        /// </summary>
        private void GameEventLoop()
        {
            while (!_stopRequested)
            {
                EventBus.DispatchEvents();
                Thread.Sleep(1000 / ConfigManager.Settings.GameEventDispatchesPerSecond);
            }
        }

        /// <summary>
        /// The NetLoop function, responsible for listening for connections, data, and disconnects
        /// </summary>
        private void NetLoop()
        {
            while (!_stopRequested)
            {
                while (_host.Service(ConfigManager.Settings.HostEventTimeout, out Event enetEvent))
                {
                    switch (enetEvent.Type)
                    {
                        case ENet.EventType.Connect:
                            ConsoleUtils.Info("Client connected on {0}", enetEvent.Peer.GetRemoteAddress());
                            Connections.Add(new ClientConnectionInfo(enetEvent.Peer));
                            break;

                        case ENet.EventType.Receive:
                            PacketHandlerManager.HandleData(enetEvent.Packet.GetBytes(), GetConnectionInfoByPeer(enetEvent.Peer));
                            enetEvent.Packet.Dispose();
                            break;

                        case ENet.EventType.Disconnect:
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
            _host.Dispose();
        }

        public ClientConnectionInfo GetConnectionInfoByPeer(Peer peer)
        {
            return Connections.First(con => con.Peer.GetRemoteAddress().Port == peer.GetRemoteAddress().Port);
        }

        public World GetWorldById(int id)
        {
            return Worlds.First(world => world.WorldId == id);
        }
    }
}
