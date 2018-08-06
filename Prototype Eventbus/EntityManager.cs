using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Eventbus
{
    class EntityManager
    {
        public EntityManager()
        {
            Program.EventBus.Subscribe<EntityEvent>(HandleEntityEvent);
        }

        public void HandleEntityEvent(EntityEvent entityEvent)
        {
            Console.WriteLine("Entity Event executed on entity {0} of type {1}", entityEvent.Entity.Name, entityEvent.Type);
        }
    }
}
