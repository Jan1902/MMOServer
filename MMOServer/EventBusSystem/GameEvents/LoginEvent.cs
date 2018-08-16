using MMOServer.Networking;

namespace MMOServer.EventBusSystem.GameEvents
{
    class LoginEvent : GameEvent
    {
        public LoginEventType EventType { get; set; }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public ClientConnectionInfo Connection { get; set; }

        public LoginEvent(LoginEventType eventType)
        {
            EventType = eventType;
        }
    }

    public enum LoginEventType
    {
        LoginRequest
    }
}
