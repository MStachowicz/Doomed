using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenGL_Game.Components
{
    class ComponentScale : IComponent
    {
        Vector3 scale;

        public ComponentScale(float x, float y, float z)
        {
            scale = new Vector3(x, y, z);
        }

        public ComponentScale(Vector3 pos)
        {
            scale = pos;
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SCALE; }
        }
    }
}