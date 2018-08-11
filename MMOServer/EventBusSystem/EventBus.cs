using MMOServer.Networking;
using System.Collections.Generic;
using System.Linq;

namespace MMOServer.EventBusSystem
{
    class EventBus
    {
        private List<IGameManager> _managers;

        private GameServer _gameServer;
        private List<GameEvent> _gameEvents = new List<GameEvent>();

        public EventBus(GameServer gameServer, List<IGameManager> managers)
        {
            _gameServer = gameServer;
            _managers = managers;
        }

        public void PublishEvent(GameEvent gameEvent)
        {
            _gameEvents.Add(gameEvent);
        }

        public void DispatchEvents()
        {
            foreach(var gameEvent in _gameEvents.ToList())
            {
                foreach(var manager in _managers.Where(m => m.HandledGameEvents.Contains(gameEvent.GetType())))
                {
                    manager.HandleGameEvent(gameEvent);
                    _gameEvents.Remove(gameEvent);
                }
            }
        }
    }
}
