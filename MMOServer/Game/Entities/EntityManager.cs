using MMOServer.ConsoleStuff;
using MMOServer.EventBusSystem;
using MMOServer.EventBusSystem.GameEvents;
using MMOServer.Networking;
using MMOServer.Other;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MMOServer.Game.Entities
{
    class EntityManager : IGameManager
    {
        private World _world;
        private GameServer _gameServer;

        public List<Entity> Entities { get; private set; }

        public List<Player> Players
        {
            get
            {
                var players = new List<Player>();
                foreach (Player player in Entities)
                    players.Add(player);
                return players;
            }
        }

        public List<Type> HandledGameEvents { get => new List<Type> { typeof(EntityEvent) }; }

        //Same thing for other entity types

        public EntityManager(World world, GameServer gameServer)
        {
            _world = world;
            _gameServer = gameServer;
            Entities = new List<Entity>();
        }

        public Entity GetEntityById(int id)
        {
            return Entities.First(e => e.EntityID == id);
        }

        private void SpawnPlayer(ClientConnectionInfo connection, Vector pos, Vector rot, string name, int level, int experience)
        {
            var player = new Player(connection, _gameServer.NextEntityID, pos, rot, name, level, experience);
            connection.Player = player;
            Entities.Add(player);

            var entitySpawnEvent = new EntityEvent(EntityEventType.EntitySpawned)
            {
                Entity = player
            };

            _gameServer.EventBus.PublishEvent(entitySpawnEvent);
            //TODO: ADD THE SET_HEALTH STUFF ASWELL
        }

        private void DestroyEntity(int entityId)
        {
            Entities.Remove(Entities.Where(e => e.EntityID == entityId).FirstOrDefault());

            var entityDestroyEvent = new EntityEvent(EntityEventType.EntityDestroy)
            {
                EntityID = entityId
            };
        }

        public void HandleGameEvent(GameEvent gameEvent)
        {
            if (gameEvent is EntityEvent entityEvent)
            {
                if (entityEvent.WorldId != _world.WorldId)
                    return;

                switch (entityEvent.EventType)
                {
                    case EntityEventType.EntitySpawnRequest:
                        if(entityEvent.EntityType == EntityType.Player)
                            SpawnPlayer(entityEvent.Connection, entityEvent.Position, entityEvent.Rotation, entityEvent.Name, entityEvent.Level, entityEvent.Experience);
                        ConsoleUtils.Info("Entity Spawn Request event handled on Entity Manager");
                        break;
                    case EntityEventType.EntityDestroyRequest:
                        DestroyEntity(entityEvent.EntityID);
                        ConsoleUtils.Info("Entity Destroy Request event handled on Entity Manager");
                        break;

                    default:
                        //Just for CodeCracker to not cry
                        break;
                }
            }
        }
    }
}
