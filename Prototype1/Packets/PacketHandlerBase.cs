using Prototype1.Networking;

namespace Prototype1.Packets
{
    abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract PacketOP Op { get; }
        public abstract void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
