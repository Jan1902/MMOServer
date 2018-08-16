using System;

namespace MMOServer.Networking.Packets
{
    abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract void Handle(byte[] data, ClientConnectionInfo connection);
    }

    sealed class PacketHandlerAttribute : Attribute
    {
        public PacketOP OP { get; set; }

        public PacketHandlerAttribute(PacketOP op)
        {
            OP = op;
        }
    }
}
