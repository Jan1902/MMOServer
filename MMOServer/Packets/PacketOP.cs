namespace MMOServer.Packets
{
    enum PacketOP : byte
    {
        HandshakeRequest = 0x00,
        HandshakeResponse = 0x01,
        EntitySpawn = 0x02
    }
}
