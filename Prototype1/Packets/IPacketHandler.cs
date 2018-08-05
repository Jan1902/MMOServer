using Prototype1.Networking;

namespace Prototype1.Packets
{
    interface IPacketHandler
    {
        void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
