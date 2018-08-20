namespace MMOServer.Networking.Packets.PacketDefinitions.CB
{
    class LoginResponse : Packet
    {
        public LoginResponseCode ResponseCode { get; set; }
        public string SceneName { get; set; }

        public LoginResponse() : base(PacketOP.LoginResponse) { }

        public override byte[] Create()
        {
            Write((byte)ResponseCode);
            Write(SceneName);
            return Bytes.ToArray();
        }
    }

    enum LoginResponseCode : byte
    {
        OK = 0,
        INVALID_DATA = 1,
        INTERNAL_ERROR = 2
    }
}
