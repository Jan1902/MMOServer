using ENet;
using MMOServer.Encryption;
using MMOServer.Game.Entities;

namespace MMOServer.Networking
{
    /// <summary>
    /// The Client Connection Info, responsible for storing network related data about the client
    /// </summary>
    class ClientConnectionInfo
    {
        public Peer Peer { get; private set; }
        public Player Player { get; set; }
        public ClientEncryptionInfo Encryption { get; private set; }

        public ClientConnectionInfo(Peer peer)
        {
            Peer = peer;
            Encryption = new ClientEncryptionInfo();
        }

        public void Disconnect()
        {
            //Send disconnect packet
            Peer.Disconnect(0);
        }
    }
}
