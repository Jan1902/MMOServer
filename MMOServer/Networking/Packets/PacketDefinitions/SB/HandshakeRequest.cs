namespace MMOServer.Networking.Packets.PacketDefinitions.SB
{
    class HandshakeRequest : Packet
    {
        public int ProtocolVersion { get; private set; }

        public HandshakeRequest(byte[] data) : base(data)
        {
            ProtocolVersion = ReadInt();
        }
    }
}
