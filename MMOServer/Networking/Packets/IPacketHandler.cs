namespace MMOServer.Networking.Packets
{
    interface IPacketHandler
    {
        void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
