using System;
using System.Net;
using System.Net.Sockets;

namespace MMOServer
{
    class GameServer
    {
        public ConnectionListener ConnectionListener { get; private set; }

        public void Init()
        {
            ConnectionListener = new ConnectionListener();
        }
    }
}
