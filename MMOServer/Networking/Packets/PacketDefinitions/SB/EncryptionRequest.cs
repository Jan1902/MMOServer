namespace MMOServer.Networking.Packets.PacketDefinitions.SB
{
    class EncryptionRequest : Packet
    {
        public byte[] PublicRSAKey { get; private set; }

        public EncryptionRequest(byte[] data) : base(data)
        {
            var keyLength = ReadInt();
            PublicRSAKey = ReadBytes(keyLength);
        }
    }
}
