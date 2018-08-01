using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    class PacketSenderManager
    {
        public static void SendHandshakeResponse(ClientConnectionInfo connection, byte responseCode) //connection == null -> broadcast
        {
            var bytes = new List<byte>();
            bytes.Add(1);
            bytes.Add(responseCode);

            if (connection != null)
                connection.Peer.Send(1, bytes.ToArray(), ENet.PacketFlags.Reliable);
            else
            {
                foreach(ClientConnectionInfo con in GameServer.Instance.Connections)
                    con.Peer.Send(1, bytes.ToArray(), ENet.PacketFlags.Reliable);
            }
        }
    }
}
