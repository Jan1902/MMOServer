namespace MMOServer.Networking.Packets
{
    enum PacketOP : byte
    {
        HandshakeRequest = 0x00,
        HandshakeResponse = 0x01,
        EncryptionRequest = 0x02,
        EncryptionResponse = 0x03,
        LoginRequest = 0x04,
        LoginResponse = 0x05,
        EntitySpawn = 0x06,
        PlayerSpawn = 0x07
    }
}
