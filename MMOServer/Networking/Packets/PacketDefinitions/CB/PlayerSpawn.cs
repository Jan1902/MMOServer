using MMOServer.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Networking.Packets.PacketDefinitions.CB
{
    class PlayerSpawn : Packet
    {
        public int EntityID { get; set; }

        public Vector Position { get; set; }
        public Vector Rotation { get; set; }

        public string Name { get; set; }

        public PlayerSpawn() : base(PacketOP.PlayerSpawn) { }

        public override byte[] Create()
        {
            Write(EntityID);

            Write(Position.X);
            Write(Position.Y);
            Write(Position.Z);

            Write(Rotation.Y);
            Write(Rotation.X);
            Write(Rotation.Z);

            Write(Name);
            return Bytes.ToArray();
        }
    }
}
