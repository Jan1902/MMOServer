using MMOServer.EventBusSystem.GameEvents;
using MMOServer.Networking.Packets.PacketDefinitions.SB;

namespace MMOServer.Networking.Packets.Handlers
{
    [PacketHandler(PacketOP.LoginRequest)]
    class LoginRequestHandler : PacketHandlerBase
    {
        private GameServer _gameServer;

        public LoginRequestHandler(GameServer gameServer)
        {
            _gameServer = gameServer;
        }

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            var loginRequest = new LoginRequest(data);
            //TODO: IMPLEMENT ACCOUNT SYSTEM AND PLAYER DATA

            var loginEvent = new LoginEvent(LoginEventType.LoginRequest)
            {
                Connection = connection,
                Username = loginRequest.Username,
                PasswordHash = loginRequest.PasswordHash
            };

            _gameServer.EventBus.PublishEvent(loginEvent);
        }
    }
}
