using MMOServer.Networking;
using MMOServer.Other;
using System.Collections.Generic;
using System.Linq;

namespace MMOServer.Game.Entities
{
    /// <summary>
    /// The Entity Manager, responsible for taking care of high level entity functions
    /// </summary>
    class EntityManager
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

        public Entity SpawnPlayer(ClientConnectionInfo connection, Vector pos, Vector rot, string name, int level, int experience)
        {
            var player = new Player(connection, _gameServer.NextEntityID, pos, rot, name, level, experience);
            connection.Player = player;
            Entities.Add(player);
            _gameServer.PacketSenderManager.SendEntitySpawn(Players.Select(p => p.Connection).ToList(), player);
            return player;
        }
    }
}
