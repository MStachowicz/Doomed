﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemPhysics : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_VELOCITY);

        public string Name
        {
            get { return "SystemPhysics"; }
        }

        public void OnAction(Entity entity)
        {
            if (entity.Name == "Drone")
            {

            }

            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent velocityComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_VELOCITY;
                });

                if (entity.Name == "Player") // update the player location component with current camera position.
                {
                    ((ComponentPosition)positionComponent).Position = MyGame.gameInstance.playerCamera.Position;
                }

                Motion((ComponentPosition)positionComponent, (ComponentVelocity)velocityComponent);
            }
        }

        private void Motion(ComponentPosition pos, ComponentVelocity vel)
        {
            pos.Position += vel.Velocity * MyGame.dt;
        }
    }
}
