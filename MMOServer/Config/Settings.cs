using System;

namespace MMOServer.Config
{
    class Settings
    {
        [Setting("Network")]
        public int MaxPlayers { get; set; } = 20;

        [Setting("Network")]
        public int Port { get; set; } = 19022;

        [Setting("Network")]
        public int HostEventTimeout { get; set; } = 20;

        [Setting("Server")]
        public int PhysicsUpdatesPerSecond { get; set; } = 10;

        [Setting("Server")]
        public int GameEventDispatchesPerSecond { get; set; } = 100;

        [Setting("Database")]
        public string DatabaseAddress { get; set; } = "localhost";

        [Setting("Database")]
        public string DatabaseName { get; set; } = "mmorpg";

        [Setting("Database")]
        public string DatabaseUsername { get; set; } = "username";

        [Setting("Database")]
        public string DatabasePassword { get; set; } = "password";
    }

    public class SettingAttribute : Attribute
    {
        public string Section { get; private set; }

        public SettingAttribute(string section)
        {
            Section = section;
        }
    }
}
