namespace MMOServer.EventBusSystem
{
    class GameEvent
    {
        public EventType EventType { get; private set; }

        public GameEvent(EventType type)
        {
            EventType = type;
        }
    }

    public enum EventType
    {
        EntityMoved,
        EntityDestroyed,
        EntitySpawned
    }
}
