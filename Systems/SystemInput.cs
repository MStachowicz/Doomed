using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using OpenTK.Input;

namespace OpenGL_Game.Systems
{
    class SystemInput : ISystem
    {
        private KeyboardState KeyStates;
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_INPUT);

        public string Name
        {
            get { return "SystemInput"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                KeyStates = Keyboard.GetState();

                //Forward and Backwards motions
                if (KeyStates.IsKeyDown(Key.W) || KeyStates.IsKeyDown(Key.Up))
                {
                    MyGame.gameInstance.playerCamera.ProcessMovement(Camera.CameraMovement.Forward);
                }
                else if (KeyStates.IsKeyDown(Key.S) || KeyStates.IsKeyDown(Key.Down))
                {
                    MyGame.gameInstance.playerCamera.ProcessMovement(Camera.CameraMovement.Backward);
                }

                //Left and Right motions
                if (KeyStates.IsKeyDown(Key.A) || KeyStates.IsKeyDown(Key.Left))
                {
                    MyGame.gameInstance.playerCamera.ProcessMovement(Camera.CameraMovement.Left);
                }
                else if (KeyStates.IsKeyDown(Key.D) || KeyStates.IsKeyDown(Key.Right))
                {
                    MyGame.gameInstance.playerCamera.ProcessMovement(Camera.CameraMovement.Right);
                }
            }
        }
    }
}
