using ENet;
using MMOServer.Config;
using MMOServer.ConsoleStuff;
using MMOServer.Database;
using MMOServer.Encryption;
using MMOServer.EventBusSystem;
using MMOServer.EventBusSystem.GameEvents;
using MMOServer.Game;
using MMOServer.Game.Entities;
using MMOServer.Networking.Packets;
using MMOServer.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MMOServer.Networking
{
    class GameServer
    {
        private Host _host;

        private bool _stopRequested;

        public List<ClientConnectionInfo> Connections { get; private set; }
        public List<World> Worlds { get; private set; }

        public PacketHandlerManager PacketHandlerManager { get; private set; }
        public PacketSenderManager PacketSenderManager { get; private set; }
        public LoginManager LoginManager { get; set; }
        public EventNotifyManager EventNotifyManager { get; set; }

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

        private void Init()
        {
            Connections = new List<ClientConnectionInfo>();
            Logger.Initialize();

            LoginManager = new LoginManager(this);
            EventNotifyManager = new EventNotifyManager(this);

            PacketHandlerManager = new PacketHandlerManager(this);
            PacketSenderManager = new PacketSenderManager(this);
            ConsoleManager = new ConsoleManager(this);
            ConfigManager = new ConfigManager();
            DatabaseManager = new DatabaseManager(this);
            EncryptionManager = new EncryptionManager();

            Worlds = new List<World>
            {
                new World(0, "Castle", this) //Just for testing
            };
            ConsoleUtils.Info("Running {0} world instances", Worlds.Count);

            var managers = new List<IGameManager>();
            managers.AddRange(Worlds.Select(w => w.EntityManager));
            managers.Add(LoginManager);
            managers.Add(EventNotifyManager);
            EventBus = new EventBus(managers);

            _host = new Host();
            _host.InitializeServer(ConfigManager.Settings.Port, ConfigManager.Settings.MaxPlayers);
            _host.SetChannelLimit(5);
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

        private void PhysicsLoop()
        {
            while(!_stopRequested)
            {
                //TODO: DO MOVEMENT CALCULATIONS HERE
                Thread.Sleep(1000 / ConfigManager.Settings.PhysicsUpdatesPerSecond);
            }
        }

        private void GameEventLoop()
        {
            while (!_stopRequested)
            {
                EventBus.DispatchEvents();
                Thread.Sleep(1000 / ConfigManager.Settings.GameEventDispatchesPerSecond);
            }
        }

        private void NetLoop()
        {
            while (!_stopRequested)
            {
                while (_host.Service(ConfigManager.Settings.HostEventTimeout, out Event enetEvent))
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
                            var destroyRequest = new EntityEvent(EntityEventType.EntityDestroyRequest)
                            {
                                EntityID = GetConnectionInfoByPeer(enetEvent.Peer).Player.EntityID
                            };
                            EventBus.PublishEvent(destroyRequest);
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

        public World GetEntitiesWorld(Entity entity)
        {
            return Worlds.First(world => world.EntityManager.Entities.Contains(entity));
        }
    }
}
