using MMOServer.ConsoleStuff;
using MMOServer.Networking;
using MMOServer.Packets.PacketDefinitions.CB;
using MMOServer.Packets.PacketDefinitions.SB;

namespace MMOServer.Packets.Handlers
{
    /// <summary>
    /// The Handshake Request Packet Handler
    /// </summary>
    class HandshakeRequestHandler : PacketHandlerBase
    {
        public override PacketOP Op => PacketOP.HandshakeRequest;

        private GameServer _gameServer;

        public HandshakeRequestHandler(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            var handshakeRequest = new HandshakeRequest(data);
            ConsoleUtils.Info("Handling a Handshake Request from client on {0}", connection.Peer.GetRemoteAddress());
            _gameServer.PacketSenderManager.SendHandshakeResponse(connection, handshakeRequest.ProtocolVersion == 1 ? HandshakeResponseCode.OK : HandshakeResponseCode.OUT_OF_DATE);
        }
    }
}
