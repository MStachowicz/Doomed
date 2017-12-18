using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentAI : IComponent
    {
        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AMMO; }
        }
    }
}
