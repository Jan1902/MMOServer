using MMOServer.Networking;

namespace MMOServer.Packets
{
    interface IPacketHandler
    {
        void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
