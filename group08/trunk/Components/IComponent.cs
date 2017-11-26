using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    [FlagsAttribute]
    enum ComponentTypes {
        COMPONENT_NONE = 0,
	    COMPONENT_POSITION = 1 << 0,
        COMPONENT_GEOMETRY = 1 << 1,
        COMPONENT_TEXTURE  = 1 << 2,
        COMPONENT_VELOCITY = 1 << 3,
        COMPONENT_AMMO = 1 << 4,
        COMPONENT_HEALTH = 1 << 5,
        COMPONENT_INPUT = 1 << 6
    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
