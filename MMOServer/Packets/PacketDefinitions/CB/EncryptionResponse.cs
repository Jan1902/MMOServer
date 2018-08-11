using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Packets.PacketDefinitions.CB
{
    class EncryptionResponse : Packet
    {
        public byte[] AESKey { get; set; }
        public EncryptionResponseCode ResponseCode { get; set; }

        public EncryptionResponse() : base(PacketOP.EncryptionResponse) { }

        public override byte[] Create()
        {
            Write((byte)ResponseCode);
            Write(AESKey);
            return CreateFinalPacket();
        }
    }

    enum EncryptionResponseCode : byte
    {
        OK = 0,
        INVALID_KEY = 1,
        INTERNAL_ERROR = 2
    }
}
