using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    /// <summary>
    /// The Client Connection Info, responsible for storing network related data about the client
    /// </summary>
    class ClientConnectionInfo
    {
        public Peer Peer { get; private set; }

        public ClientConnectionInfo(Peer peer)
        {
            Peer = peer;
        }
    }
}
