using Prototype1.Networking;
using Prototype1.Other;
using Prototype1.Packets.PacketDefinitions.CB;
using Prototype1.Packets.PacketDefinitions.SB;
using System;

namespace Prototype1.Packets.Handlers
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
            Console.WriteLine("Handling a Handshake Request from client on {0}", connection.Peer.GetRemoteAddress());
            _gameServer.PacketSenderManager.SendHandshakeResponse(connection, handshakeRequest.ProtocolVersion == 1 ? HandshakeResponseCode.OK : HandshakeResponseCode.OUT_OF_DATE);

            //just for testing
            _gameServer.Worlds[0].EntityManager.SpawnPlayer(connection, Vector.Zero(), Vector.Zero(), "Jeff", 1, 0);
        }
    }
}
