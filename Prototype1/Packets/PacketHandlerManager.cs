using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prototype1
{
    /// <summary>
    /// The Packet Handling Manager, responsible for handling packets received
    /// </summary>
    class PacketHandlerManager
    {
        private Dictionary<PacketOP, IPacketHandler> _handlers;

        public void SetupHandlers()
        {
             _handlers = new Dictionary<PacketOP, IPacketHandler>();
            foreach(PacketHandlerBase handler in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(PacketHandlerBase)).Select(t => (PacketHandlerBase)Activator.CreateInstance(t)))
            {
                _handlers.Add(handler.Op, handler);
            }
            Console.WriteLine("Set up {0} handled packets", _handlers.Count);
        }

        public void HandleData(byte[] data, ClientConnectionInfo connection)
        {
            if (!_handlers.ContainsKey((PacketOP)data[0]))
            {
                Console.WriteLine("Received invalid packet from client on {0} with Packet OP {1}", connection.Peer.GetRemoteAddress().ToString(), data[0]);
                return;
            }

            _handlers[(PacketOP)data[0]].Handle(data, connection);
        }
    }
}
