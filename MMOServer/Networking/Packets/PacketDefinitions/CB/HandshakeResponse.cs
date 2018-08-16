namespace MMOServer.Networking.Packets.PacketDefinitions.CB
{
    class HandshakeResponse : Packet
    {
        public HandshakeResponseCode ResponseCode { get; set; }

        public HandshakeResponse() : base(PacketOP.HandshakeResponse) { }

        public override byte[] Create()
        {
            Write((byte)ResponseCode);
            return Bytes.ToArray();
        }
    }

    enum HandshakeResponseCode : byte
    {
        OK = 0,
        INVALID_VERSION = 1,
        INVALID = 2,
        INTERNAL_ERROR = 3
    }
}
