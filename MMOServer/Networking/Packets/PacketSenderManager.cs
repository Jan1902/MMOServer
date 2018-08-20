using ENet;
using MMOServer.ConsoleStuff;
using MMOServer.Game.Entities;
using MMOServer.Networking.Packets.PacketDefinitions.CB;
using MMOServer.Other;
using System;
using System.Collections.Generic;

namespace MMOServer.Networking.Packets
{
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

            DefaultSend(connection, handShakeResponse.Create(), ChannelID.Handshaking, true);
        }

        public void SendEncryptionResponse(ClientConnectionInfo connection, EncryptionResponseCode responseCode, byte[] key, byte[] iv)
        {
            var handShakeResponse = new EncryptionResponse
            {
                ResponseCode = responseCode,
                AESKey = key,
                IV = iv
            };

            if (responseCode == EncryptionResponseCode.OK)
                RSAEncryptedSend(connection, handShakeResponse.Create(), ChannelID.Handshaking, true);
            else
                DefaultSend(connection, handShakeResponse.Create(), ChannelID.Handshaking, true);
        }

        private void DefaultSend(ClientConnectionInfo connection, byte[] bytes, ChannelID channel, bool reliable)
        {
            try
            {
                if (connection != null)
                {
                    if (connection.Encryption.AESEncryptionEnabled)
                        connection.Peer.Send((byte)channel, _gameServer.EncryptionManager.EncryptDataAES(bytes), reliable ? PacketFlags.Reliable : PacketFlags.None);
                    else
                        connection.Peer.Send((byte)channel, bytes, reliable ? PacketFlags.Reliable : PacketFlags.None);
                }
                else
                {
                    foreach (ClientConnectionInfo con in _gameServer.Connections)
                    {
                        if (con.Encryption.AESEncryptionEnabled)
                            con.Peer.Send((byte)channel, _gameServer.EncryptionManager.EncryptDataAES(bytes), reliable ? PacketFlags.Reliable : PacketFlags.None);
                        else
                            con.Peer.Send((byte)channel, bytes, reliable ? PacketFlags.Reliable : PacketFlags.None);
                    }
                }
            }
            catch(Exception e)
            {
                ConsoleUtils.Warning("Error sending packet");
                Logger.LogError(e.GetBaseException().ToString());
            }
        }

        private void GroupSend(List<ClientConnectionInfo> connections, byte[] bytes, ChannelID channel, bool reliable)
        {
            if(connections != null)
            {
                foreach(ClientConnectionInfo connection in connections)
                {
                    if (connection.Encryption.AESEncryptionEnabled)
                    {
                        var value = _gameServer.EncryptionManager.EncryptDataAES(bytes);
                        connection.Peer.Send((byte)channel, _gameServer.EncryptionManager.EncryptDataAES(bytes), reliable ? PacketFlags.Reliable : PacketFlags.None);
                    }
                    else
                        connection.Peer.Send((byte)channel, bytes, reliable ? PacketFlags.Reliable : PacketFlags.None);
                }
            }
            else
            {
                foreach (ClientConnectionInfo connection in _gameServer.Connections)
                {
                    if (connection.Encryption.AESEncryptionEnabled)
                        connection.Peer.Send((byte)channel, _gameServer.EncryptionManager.EncryptDataAES(bytes), reliable ? PacketFlags.Reliable : PacketFlags.None);
                    else
                        connection.Peer.Send((byte)channel, bytes, reliable ? PacketFlags.Reliable : PacketFlags.None);
                }
            }
        }

        private void RSAEncryptedSend(ClientConnectionInfo connection, byte[] bytes, ChannelID channel, bool reliable)
        {
            connection.Peer.Send((byte)channel, connection.Encryption.EncryptData(bytes), reliable ? PacketFlags.Reliable : PacketFlags.None);
        }

        public void SendEntitySpawn(List<ClientConnectionInfo> connections, Entity entity)
        {
            var entitySpawn = new EntitySpawn
            {
                EntityID = entity.EntityID,
                Position = entity.Position, //TODO: CREATE PROPER PACKET STRUCTURE FOR THIS
                Rotation = entity.Rotation,
            };

            GroupSend(connections, entitySpawn.Create(), ChannelID.Gameplay, true);

            ConsoleUtils.Info("Broadcasted entity spawn");
        }

        public void SendPlayerSpawn(List<ClientConnectionInfo> connections, Player player)
        {
            var entitySpawn = new PlayerSpawn
            {
                EntityID = player.EntityID,
                Position = player.Position,
                Rotation = player.Rotation,
                Name = player.Username
            };

            GroupSend(connections, entitySpawn.Create(), ChannelID.Gameplay, true);

            ConsoleUtils.Info("Broadcasted player spawn");
        }

        public void SendLoginResponse(ClientConnectionInfo connection, LoginResponseCode responseCode, string sceneName)
        {
            var loginResponse = new LoginResponse
            {
                ResponseCode = responseCode,
                SceneName = sceneName,
            };

            DefaultSend(connection, loginResponse.Create(), ChannelID.Handshaking, true);
        }
    }
}
