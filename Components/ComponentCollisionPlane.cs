using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentCollisionPlane : IComponent
    {
        Vector3 area;

        public ComponentCollisionPlane(float x, float y, float z)
        {
            area = new Vector3(x, y, z);
        }

        public ComponentCollisionPlane(Vector3 a)
        {
            area = a;
        }

        public Vector3 Area
        {
            get { return area; }
            set { area = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_COLLISION_PLANE; }
        }
    }
}
