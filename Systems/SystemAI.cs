using OpenGL_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenTK;
using static OpenGL_Game.Components.ComponentAI;

namespace OpenGL_Game.Systems
{
    class SystemAI : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_AI | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_ROTATION);

        public string Name
        {
            get { return "SystemAI"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                ComponentPosition positionComponent = (ComponentPosition)components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector2 position = positionComponent.Position.Xy;

                ComponentAI aiComponent = (ComponentAI)components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AI;
                });
                AIStates state = aiComponent.CurrentState;

                ComponentRotation rotationComponent = (ComponentRotation)components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
                });
                Vector2 currentRotation = rotationComponent.Rotation.Xz;
            }
        }
    }
}
