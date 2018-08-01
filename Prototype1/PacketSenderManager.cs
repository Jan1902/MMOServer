using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    class PacketSenderManager
    {
        public void SendHandshakeResponse(ClientConnectionInfo connection, byte responseCode) //connection == null -> broadcast
        {
            if(connection != null)
            {
                List<byte> bytes = new List<byte>();
                bytes.Add(1);
                bytes.Add(responseCode);
                connection.Peer.Send(1, bytes.ToArray(), ENet.PacketFlags.Reliable);
            }
        }
    }
}
