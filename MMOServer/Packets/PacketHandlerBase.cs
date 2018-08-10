using MMOServer.Networking;

namespace MMOServer.Packets
{
    abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract PacketOP Op { get; }
        public abstract void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
