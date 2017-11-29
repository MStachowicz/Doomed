using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;

namespace OpenGL_Game.Systems
{
    class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_GEOMETRY | 
            ComponentTypes.COMPONENT_TEXTURE | ComponentTypes.COMPONENT_SCALE);

        protected int pgmID;
        protected int vsID;
        protected int fsID;

        // Vertex shader attributes
        protected int attribute_vpos;
        protected int attribute_vnormal;
        protected int attribute_vtex;

        protected int uniform_mModel;
        protected int uniform_mView;
        protected int uniform_mProjection;

        // Fragment shader attributes
        protected int uniform_material;
        protected int uniform_light;
        protected int uniform_viewPos;
        protected int uniform_stex;

        public SystemRender()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vnormal = GL.GetAttribLocation(pgmID, "a_Normal");
            attribute_vtex = GL.GetAttribLocation(pgmID, "a_TexCoord");

            uniform_mModel = GL.GetUniformLocation(pgmID, "model");
            uniform_mView = GL.GetUniformLocation(pgmID, "view");
            uniform_mProjection = GL.GetUniformLocation(pgmID, "projection");

            uniform_material = GL.GetUniformLocation(pgmID, "material");
            uniform_light = GL.GetUniformLocation(pgmID, "light.position");
            uniform_viewPos = GL.GetUniformLocation(pgmID, "viewPos");
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");

            if (attribute_vpos == -1 || attribute_vtex == -1 || attribute_vnormal == -1 ||
                uniform_stex == -1 || uniform_mModel == -1 || uniform_mView == -1 || uniform_mProjection == -1)
            {
                Console.WriteLine("Error binding attributes");
                Console.WriteLine("attribute_vpos " + attribute_vpos);
                Console.WriteLine("attribute_vnormal " + attribute_vnormal);
                Console.WriteLine("attribute_vtex " + attribute_vtex);

                Console.WriteLine("uniform_mModel " + uniform_mModel);
                Console.WriteLine("uniform_mView " + uniform_mView);
                Console.WriteLine("uniform_mProjection " + uniform_mProjection);

                Console.WriteLine("uniform_material " + uniform_material);
                Console.WriteLine("uniform_light " + uniform_light);
                Console.WriteLine("uniform_viewPos " + uniform_viewPos);
                Console.WriteLine("uniform_stex " + uniform_stex);
            }
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                #region Model matrix set up

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });
                Vector3 position = ((ComponentPosition)positionComponent).Position;

                IComponent rotationComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
                });
                Vector3 rotation = ((ComponentRotation)rotationComponent).Rotation;

                IComponent scaleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SCALE;
                });
                Vector3 scale = ((ComponentScale)scaleComponent).Scale;

                // Combine transformations to create the model matrix
                Matrix4 mModel = Matrix4.CreateScale(scale) * Matrix4.CreateRotationX(rotation.X) * 
                    Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * 
                    Matrix4.CreateTranslation(position);

                #endregion

                IComponent textureComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_TEXTURE;
                });
                int texture = ((ComponentTexture)textureComponent).Texture;

                Draw(mModel, geometry, texture);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry, int texture)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Enable(EnableCap.Texture2D);

            // Setting the matrices/vectors to perform shader light calculations.
            // Model
            Matrix4 mModel = model;
            GL.UniformMatrix4(uniform_mModel, false, ref mModel);
            // View
            Matrix4 mView = MyGame.gameInstance.playerCamera.getViewMatrix();
            GL.UniformMatrix4(uniform_mView, false, ref mView);
            //Projection
            Matrix4 mProjection = MyGame.gameInstance.projection;
            GL.UniformMatrix4(uniform_mProjection, false, ref mProjection);
            //view position
            Vector3 mViewPos = MyGame.gameInstance.playerCamera.Position;
            GL.Uniform3(uniform_mProjection, ref mViewPos);


            geometry.Render();

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }
    }
}
