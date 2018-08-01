using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    interface IPacketHandler
    {
        void Handle(byte[] data, ClientConnectionInfo connection);
    }
}
