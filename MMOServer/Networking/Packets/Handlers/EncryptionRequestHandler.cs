using MMOServer.Networking.Packets.PacketDefinitions.CB;
using MMOServer.Networking.Packets.PacketDefinitions.SB;

namespace MMOServer.Networking.Packets.Handlers
{
    [PacketHandler(PacketOP.EncryptionRequest)]
    class EncryptionRequestHandler : PacketHandlerBase
    {
        private GameServer _gameServer;

        public EncryptionRequestHandler(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            var encryptionRequest = new EncryptionRequest(data);
            if(encryptionRequest.PublicRSAKey.Length < 32)
            {
                _gameServer.PacketSenderManager.SendEncryptionResponse(connection, EncryptionResponseCode.INVALID_KEY, new byte[] { 0 }, new byte[] { 0 });
                return;
            }
            connection.Encryption.EnableRSAEncryption(encryptionRequest.PublicRSAKey);
            _gameServer.PacketSenderManager.SendEncryptionResponse(connection, EncryptionResponseCode.OK, _gameServer.EncryptionManager.GetKeyAES(), _gameServer.EncryptionManager.GetIVAes());
            connection.Encryption.DisableRSAEncryption();
            connection.Encryption.AESEncryptionEnabled = true;
        }
    }
}
