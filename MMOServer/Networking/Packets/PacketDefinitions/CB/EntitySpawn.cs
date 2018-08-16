using MMOServer.Other;

namespace MMOServer.Networking.Packets.PacketDefinitions.CB
{
    class EntitySpawn : Packet
    {
        public int EntityID { get; set; }

        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public EntitySpawn() : base(PacketOP.EntitySpawn) { }

        public override byte[] Create()
        {
            Write(EntityID);

            Write(Position.X);
            Write(Position.Y);
            Write(Position.Z);

            Write(Rotation.Y);
            Write(Rotation.X);
            Write(Rotation.Z);
            return Bytes.ToArray();
        }
    }
}
