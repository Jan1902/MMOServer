using MMOServer.ConsoleStuff;
using MMOServer.Networking;
using MMOServer.Packets.PacketDefinitions.CB;
using MMOServer.Packets.PacketDefinitions.SB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Packets.Handlers
{
    class EncryptionRequestHandler : PacketHandlerBase
    {
        public override PacketOP Op => PacketOP.HandshakeRequest;

        private GameServer _gameServer;

        public EncryptionRequestHandler(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            var encryptionRequest = new EncryptionRequest(data);
            ConsoleUtils.Info("Handling an Encryption Request from client on {0}", connection.Peer.GetRemoteAddress());
            connection.Encryption.EnableRSAEncryption(encryptionRequest.PublicRSAKey);
            _gameServer.PacketSenderManager.SendEncryptionResponse(connection, EncryptionResponseCode.OK, _gameServer.EncryptionManager.GetKeyAES());
            connection.Encryption.DisableRSAEncryption();
            connection.Encryption.AESEncryptionEnabled = true;
        }
    }
}
