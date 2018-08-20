using MMOServer.Game.Entities;
using MMOServer.Networking;
using MMOServer.Other;

namespace MMOServer.EventBusSystem.GameEvents
{
    class EntityEvent : GameEvent
    {
        public EntityEventType EventType { get; set; }

        public Entity Entity { get; set; }

        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public int EntityID { get; set; }
        public string Name { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public ClientConnectionInfo Connection { get; set; }
        public EntityType EntityType { get; set; }

        public int WorldId { get; set; }

        public EntityEvent(EntityEventType type)
        {
            EventType = type;
        }
    }

    public enum EntityEventType
    {
        EntityMoved,
        EntityDestroyed,
        EntitySpawnRequest,
        EntitySpawned,
        EntityDestroyRequest,
        EntityDestroy
    }
}
