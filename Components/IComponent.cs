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
        COMPONENT_LIGHT = 1 << 6,
        COMPONENT_WEAPON = 1 << 7,
        COMPONENT_COLLISION_SPHERE = 1 << 8,
        COMPONENT_COLLISION_PLANE = 1 << 9,
        COMPONENT_INPUT = 1 << 10




    }

    interface IComponent
    {
        ComponentTypes ComponentType
        {
            get;
        }
    }
}
