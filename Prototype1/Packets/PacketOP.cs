using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    enum PacketOP : byte
    {
        HandshakeRequest = 0x00,
        HandshakeResponse = 0x01
    }
}
