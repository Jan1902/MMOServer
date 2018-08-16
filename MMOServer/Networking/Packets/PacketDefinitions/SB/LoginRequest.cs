namespace MMOServer.Networking.Packets.PacketDefinitions.SB
{
    class LoginRequest : Packet
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public LoginRequest(byte[] data) : base(data)
        {
            Username = ReadString();
            var passwordLength = ReadInt();
            PasswordHash = ReadBytes(passwordLength);
        }
    }
}
