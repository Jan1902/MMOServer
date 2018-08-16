using MMOServer.Networking.Packets.PacketDefinitions.CB;
using MMOServer.Networking.Packets.PacketDefinitions.SB;

namespace MMOServer.Networking.Packets.Handlers
{
    [PacketHandler(PacketOP.HandshakeRequest)]
    class HandshakeRequestHandler : PacketHandlerBase
    {
        private GameServer _gameServer;

        public HandshakeRequestHandler(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            var handshakeRequest = new HandshakeRequest(data);
            _gameServer.PacketSenderManager.SendHandshakeResponse(connection, handshakeRequest.ProtocolVersion == 1 ? HandshakeResponseCode.OK : HandshakeResponseCode.INVALID_VERSION);
        }
    }
}
