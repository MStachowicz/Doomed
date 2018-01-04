using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL_Game.Systems
{
    class SystemLighting : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_LIGHT_EMITTER);

        public static int lightIndex; // Assigned and incremented by every light emitter component created.

        public SystemLighting() {}

        /// <summary>
        /// Sets the uniform values for all the light entities in the shader used in SystemRender.
        /// </summary>
        /// <param name="entity"></param>
        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                IComponent lightComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_LIGHT_EMITTER;
                });

                int lightIndex = ((ComponentLightEmitter)lightComponent).Index;

                GL.UseProgram(SystemRender.pgmID);

                int uniform_position = GL.GetUniformLocation(SystemRender.pgmID, ("lights[" + lightIndex + "].position"));
                Vector3 mPosition = ((ComponentPosition)positionComponent).Position;
                GL.Uniform3(uniform_position, ref mPosition);

                int uniform_Ambient = GL.GetUniformLocation(SystemRender.pgmID, ("lights[" + lightIndex + "].ambient"));
                Vector3 mAmbient = ((ComponentLightEmitter)lightComponent).Ambient;
                GL.Uniform3(uniform_Ambient, ref mAmbient);

                int uniform_Diffuse = GL.GetUniformLocation(SystemRender.pgmID, ("lights[" + lightIndex + "].diffuse"));
                Vector3 mDiffuse = ((ComponentLightEmitter)lightComponent).Diffuse;
                GL.Uniform3(uniform_Diffuse, ref mDiffuse);

                int uniform_Specular = GL.GetUniformLocation(SystemRender.pgmID, ("lights[" + lightIndex + "].specular"));
                Vector3 mSpecular = ((ComponentLightEmitter)lightComponent).Specular;
                GL.Uniform3(uniform_Specular, ref mSpecular);
            }
        }

        public string Name
        {
            get { return "SystemLighting"; }
        }
    }
}
