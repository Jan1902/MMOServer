using MMOServer.ConsoleStuff;
using MMOServer.EventBusSystem;
using MMOServer.EventBusSystem.GameEvents;
using MMOServer.Networking;
using MMOServer.Networking.Packets.PacketDefinitions.CB;
using MMOServer.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MMOServer.Database
{
    class LoginManager : IGameManager
    {
        public List<Type> HandledGameEvents => new List<Type> { typeof(LoginEvent) };

        private GameServer _gameServer;

        public LoginManager(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public void HandleGameEvent(GameEvent gameEvent)
        {
            if(gameEvent is LoginEvent loginEvent)
            {
                var account = _gameServer.DatabaseManager.GetAccountFromUsername(loginEvent.Username);
                var hash = new List<byte>();
                hash.AddRange(account.PasswordSalt);
                hash.AddRange(loginEvent.PasswordHash);

                if (Enumerable.SequenceEqual(account.PasswordHash, SHA256.Create().ComputeHash(hash.ToArray())))
                {
                    ConsoleUtils.Info("Client on {0} has logged in as {1}", loginEvent.Connection.Peer.GetRemoteAddress(), loginEvent.Username);
                    var entityEvent = new EntityEvent(EntityEventType.EntitySpawned)
                    {
                        Connection = loginEvent.Connection,
                        Position = Vector.Zero(),
                        Rotation = Vector.Zero(),
                        Level = 1,
                        Experience = 0,
                        WorldId = 0
                    };
                    _gameServer.PacketSenderManager.SendLoginResponse(loginEvent.Connection, LoginResponseCode.OK, "Castle", 1); //Do all of this properly
                    _gameServer.EventBus.PublishEvent(entityEvent);
                }
                else
                {
                    _gameServer.PacketSenderManager.SendLoginResponse(loginEvent.Connection, LoginResponseCode.INVALID_DATA, "", 0); //Do all of this properly
                }
            }
        }
    }
}
