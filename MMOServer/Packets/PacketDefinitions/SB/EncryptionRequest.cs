using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Packets.PacketDefinitions.SB
{
    class EncryptionRequest : Packet
    {
        public byte[] PublicRSAKey { get; set; }

        public EncryptionRequest(byte[] data) : base(PacketOP.EncryptionRequest, data)
        {
            var keyLength = ReadInt();
            PublicRSAKey = ReadBytes(keyLength);
        }
    }
}
