using ENet;
using Prototype1.Game.Entities;

namespace Prototype1.Networking
{
    /// <summary>
    /// The Client Connection Info, responsible for storing network related data about the client
    /// </summary>
    class ClientConnectionInfo
    {
        public Peer Peer { get; private set; }
        public Player Player { get; set; }

        public ClientConnectionInfo(Peer peer)
        {
            Peer = peer;
        }

        public void Disconnect()
        {
            //Send disconnect packet
            Peer.Disconnect(0);
        }
    }
}
