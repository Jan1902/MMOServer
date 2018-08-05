using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestingClient
{
    class Program
    {
        static void Main()
        {
            var host = new Host();
            host.InitializeClient(1);

            var peer = host.Connect("127.0.0.1", 19022, 0);

            while (host.Service(20, out Event enetEvent))
            {
                switch (enetEvent.Type)
                {
                    case EventType.Connect:
                        Console.WriteLine("connected");
                        peer.Send(1, new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00 }, PacketFlags.Reliable);
                        break;
                    case EventType.Receive:
                        Console.WriteLine("received {0} bytes from server", enetEvent.Packet.GetBytes().Length);
                        //handle the data
                        enetEvent.Packet.Dispose();
                        break;
                    default:
                        Console.WriteLine("invalid event");
                        break;
                }
            }
            Console.ReadLine();
            host.Dispose();
            peer.Disconnect(0);
        }
    }
}
