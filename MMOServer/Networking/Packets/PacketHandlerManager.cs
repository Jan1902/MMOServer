using MMOServer.ConsoleStuff;
using MMOServer.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MMOServer.Networking.Packets
{
    class PacketHandlerManager
    {
        private Dictionary<PacketOP, IPacketHandler> _handlers;

        private GameServer _gameServer;

        public PacketHandlerManager(GameServer gameServer)
        {
            _gameServer = gameServer;
            SetupHandlers();
        }

        private void SetupHandlers()
        {
             _handlers = new Dictionary<PacketOP, IPacketHandler>();
            foreach(Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(PacketHandlerAttribute)).Count() != 0))
            {
                _handlers.Add(((PacketHandlerAttribute)type.GetCustomAttributes(typeof(PacketHandlerAttribute)).First()).OP, (PacketHandlerBase)Activator.CreateInstance(type, _gameServer));
            }
            ConsoleUtils.Info("Set up {0} handled packets", _handlers.Count);
        }

        public void HandleData(byte[] data, ClientConnectionInfo connection)
        {
            if (connection.Encryption.AESEncryptionEnabled)
                data = _gameServer.EncryptionManager.DecryptDataAES(data);

            if (!_handlers.ContainsKey((PacketOP)data[0]))
            {
                ConsoleUtils.Warning("Received invalid packet from client on {0} with Packet OP {1}", connection.Peer.GetRemoteAddress(), data[0]);
                return;
            }

            try
            {
                _handlers[(PacketOP)data[0]].Handle(data, connection);
            }
            catch(Exception e)
            {
                ConsoleUtils.Warning("An error occured while trying to read {0} packet from client at {1}", (PacketOP)data[0], connection.Peer.GetRemoteAddress());
                Logger.LogError(e.GetBaseException().ToString());
            }
        }
    }
}
