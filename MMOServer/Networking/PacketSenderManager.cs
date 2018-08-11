using ENet;
using MMOServer.ConsoleStuff;
using MMOServer.Game.Entities;
using MMOServer.Packets.PacketDefinitions.CB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMOServer.Networking
{
    /// <summary>
    /// The Packet Sender Manager, responsible for sending and broadcasting packets
    /// </summary>
    class PacketSenderManager
    {
        private GameServer _gameServer;

        public PacketSenderManager(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public void SendHandshakeResponse(ClientConnectionInfo connection, HandshakeResponseCode responseCode)
        {
            var handShakeResponse = new HandshakeResponse
            {
                ResponseCode = responseCode
            };

            DefaultSend(connection, handShakeResponse.Create());

            ConsoleUtils.Info("Sent Handshake Response to client on {0}", connection.Peer.GetRemoteAddress());
        }

        public void SendEncryptionResponse(ClientConnectionInfo connection, EncryptionResponseCode responseCode, byte[] key)
        {
            var handShakeResponse = new EncryptionResponse
            {
                ResponseCode = responseCode,
                AESKey = key
            };

            if (responseCode == EncryptionResponseCode.OK)
                RSAEncryptedSend(connection, handShakeResponse.Create());
            else
                DefaultSend(connection, handShakeResponse.Create());

            ConsoleUtils.Info("Sent Encryption Response to client on {0}", connection.Peer.GetRemoteAddress());
        }

        private void DefaultSend(ClientConnectionInfo connection, byte[] bytes)
        {
            if (connection != null)
            {
                if(connection.Encryption.AESEncryptionEnabled)
                    connection.Peer.Send(1, _gameServer.EncryptionManager.EncryptDataAES(bytes), PacketFlags.Reliable);
                else
                    connection.Peer.Send(1, bytes, PacketFlags.Reliable);
            }
            else
            {
                foreach (ClientConnectionInfo con in _gameServer.Connections)
                {
                    if (con.Encryption.AESEncryptionEnabled)
                        con.Peer.Send(1, _gameServer.EncryptionManager.EncryptDataAES(bytes), PacketFlags.Reliable);
                    else
                        con.Peer.Send(1, bytes, PacketFlags.Reliable);
                }
            }
        }

        private void RSAEncryptedSend(ClientConnectionInfo connection, byte[] bytes)
        {
            connection.Peer.Send(1, connection.Encryption.EncryptData(bytes), PacketFlags.Reliable);
        }

        public void SendEntitySpawn(List<ClientConnectionInfo> connections, Entity entity)
        {
            var entitySpawn = new EntitySpawn
            {
                EntityID = entity.EntityID,
                Position = entity.Position,
                Rotation = entity.Rotation
            };

            if (connections != null)
            {
                foreach (ClientConnectionInfo con in connections)
                    con.Peer.Send(1, entitySpawn.Create(), PacketFlags.Reliable);
            }
            else
            {
                foreach (ClientConnectionInfo con in _gameServer.Connections)
                    con.Peer.Send(1, entitySpawn.Create(), PacketFlags.Reliable);
            }

            ConsoleUtils.Info("Broadcasted entity spawn");
        }
    }
}
