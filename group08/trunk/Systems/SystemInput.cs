using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;

namespace OpenGL_Game.Systems
{
    class SystemInput : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        public string Name
        {
            get { return "SystemInput"; }
        }

        public void OnAction(Entity entity)
        {
        }
    }
}
