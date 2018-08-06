using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype_Eventbus
{
    class Entity
    {
        //just for demonstration
        public string Name { get; private set; }

        public Entity(string name)
        {
            Name = name;
        }
    }
}
