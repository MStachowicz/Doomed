using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentHealth : IComponent
    {
        float health;

        public ComponentHealth(int h)
        {
            health = h;
        }

       
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AMMO; }
        }
    }
}
