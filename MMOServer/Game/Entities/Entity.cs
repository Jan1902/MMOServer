using MMOServer.Other;

namespace MMOServer.Game.Entities
{
    class Entity
    {
        public int EntityID { get; set; }

        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }
    }

    public enum EntityType
    {
        Player,
        Mob,
        NPC,
        Static
    }
}
