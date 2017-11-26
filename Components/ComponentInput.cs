<<<<<<< .mine
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentInput : IComponent
    {
        bool input;

        public ComponentInput(int i)
        {
            input = i;
        }

       
        public bool Input
        {
            get { return input; }
            set { input = value; }
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_INPUT; }
        }
    }
}
||||||| .r0
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGL_Game.Components
{
    class ComponentInput : IComponent
    {
        public ComponentTypes ComponentType
        {
            get
            {
                return ComponentTypes.COMPONENT_INPUT;
            }
        }
    }
}
>>>>>>> .r207
