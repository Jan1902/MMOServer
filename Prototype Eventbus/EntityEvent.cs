using Redbus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Eventbus
{
    class EntityEvent : EventBase
    {
        public EntityEventType Type { get; private set; }
        public Entity Entity { get; private set; }

        public enum EntityEventType
        {
            Spawn,
            Move,
            Destroy
        }

        public EntityEvent(EntityEventType type, Entity entity)
        {
            Type = type;
            Entity = entity;
        }
    }
}
