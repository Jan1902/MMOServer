namespace MMOServer.Networking.Packets.PacketDefinitions.CB
{
    class EncryptionResponse : Packet
    {
        public byte[] AESKey { get; set; }
        public byte[] IV { get; set; }
        public EncryptionResponseCode ResponseCode { get; set; }

        public EncryptionResponse() : base(PacketOP.EncryptionResponse) { }

        public override byte[] Create()
        {
            Write((byte)ResponseCode);
            Write(AESKey.Length);
            Write(AESKey);
            Write(IV.Length);
            Write(IV);
            return Bytes.ToArray();
        }
    }

    enum EncryptionResponseCode : byte
    {
        OK = 0,
        INVALID_KEY = 1,
        INTERNAL_ERROR = 2
    }
}
