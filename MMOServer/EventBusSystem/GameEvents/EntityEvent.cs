using MMOServer.Networking;
using MMOServer.Other;

namespace MMOServer.EventBusSystem.GameEvents
{
    class EntityEvent : GameEvent
    {
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public int EntityID { get; set; }
        public string Name { get; set; }
        public ClientConnectionInfo Connection { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public int WorldId { get; set; }

        public EntityEvent(EventType type) : base (type) { }
    }
}
