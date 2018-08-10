namespace MMOServer.Packets.PacketDefinitions.SB
{
    class HandshakeRequest : Packet
    {
        public int ProtocolVersion { get; private set; }

        public HandshakeRequest(byte[] data) : base(PacketOP.HandshakeRequest, data)
        {
            ProtocolVersion = ReadInt();
        }
    }
}
