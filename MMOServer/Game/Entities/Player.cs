using MMOServer.Networking;
using MMOServer.Other;

namespace MMOServer.Game.Entities
{
    class Player : Entity
    {
        public string Username { get; private set; }

        public int Level { get; private set; }
        public int Experience { get; private set; }

        public ClientConnectionInfo Connection { get; private set; }

        //inventory object of some sort

        public Player(ClientConnectionInfo connection, int entityId, Vector position, Vector rotation, string username, int level, int experience)
        {
            Connection = connection;
            Position = position;
            Rotation = rotation;
            EntityID = entityId;

            Username = username;
            Level = level;
            Experience = experience;
        }
    }
}
