using System;
using System.Collections.Generic;
using System.Linq;
using MMOServer.EventBusSystem;
using MMOServer.EventBusSystem.GameEvents;
using MMOServer.Game.Entities;

namespace MMOServer.Networking
{
    class EventNotifyManager : IGameManager
    {
        private GameServer _gameServer;

        public List<Type> HandledGameEvents => new List<Type> { typeof(EntityEvent) };

        public EventNotifyManager(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public void HandleGameEvent(GameEvent gameEvent)
        {
            if(gameEvent is EntityEvent entityEvent)
            {
                if(entityEvent.Entity is Player player)
                    _gameServer.PacketSenderManager.SendPlayerSpawn(_gameServer.GetEntitiesWorld(player).EntityManager.Players.Select(p => p.Connection).ToList(), player);
            }
        }
    }
}
