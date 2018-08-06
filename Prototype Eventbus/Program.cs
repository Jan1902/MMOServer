using Redbus;
using Redbus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Prototype_Eventbus.EntityEvent;

namespace Prototype_Eventbus
{
    class Program
    {
        public static readonly IEventBus EventBus = new EventBus();

        static void Main()
        {
            var entityManager = new EntityManager();

            Console.ReadLine();
            EventBus.Publish(new EntityEvent(EntityEventType.Spawn, new Entity("Jeff")));
            Console.ReadLine();
        }
    }
}
