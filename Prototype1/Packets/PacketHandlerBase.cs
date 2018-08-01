using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    abstract class PacketHandlerBase : IPacketHandler
    {
        public abstract PacketOP Op { get; }
        public abstract void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
