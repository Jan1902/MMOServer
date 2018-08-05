namespace Prototype1.Packets.PacketDefinitions.CB
{
    class HandshakeResponse : Packet
    {
        public HandshakeResponseCode ResponseCode { get; set; }

        public HandshakeResponse() : base(PacketOP.HandshakeResponse) { }

        public override byte[] Create()
        {
            Write((byte)ResponseCode);
            return CreateFinalPacket();
        }
    }

    enum HandshakeResponseCode : byte
    {
        OK = 0,
        OUT_OF_DATE = 1,
        INVALID = 2,
        INTERNAL_ERROR = 3
    }
}
