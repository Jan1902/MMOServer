using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1.Packets.Handlers
{
    class HandshakeRequestHandler : PacketHandlerBase
    {
        public override PacketOP Op => PacketOP.HandshakeRequest;

        public override void Handle(byte[] data, ClientConnectionInfo connection)
        {
            int protocolVersion = BitConverter.ToInt32(data, 1);
            Console.WriteLine("Handling a Handshake Request from client on {0} with protocol version {1}", connection.Peer.GetRemoteAddress(), protocolVersion);
            GameServer.Instance.PacketSenderManager.SendHandshakeResponse(connection, protocolVersion == 1 ? (byte)1 : (byte)0);
        }
    }
}
