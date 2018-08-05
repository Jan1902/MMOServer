using Prototype1.Game.Entities;
using Prototype1.Networking;

namespace Prototype1.Game
{
    class World
    {
        public int WorldId { get; private set; }
        public string WorldName { get; private set; }

        public EntityManager EntityManager { get; private set; }

        private GameServer _gameServer;

        public World(int worldId, string worldName, GameServer gameServer)
        {
            WorldId = worldId;
            WorldName = worldName;
            _gameServer = gameServer;
            EntityManager = new EntityManager(this, _gameServer);
        }
    }
}
