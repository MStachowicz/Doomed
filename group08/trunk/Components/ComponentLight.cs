using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentLight : IComponent
    {
        int intensity;
     

        public ComponentLight(int a)
        {
            intensity = a;
        }

       
        public int Intensity
        {
            get { return intensity; }
            set { intensity = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_LIGHT; }
        }
    }
}
