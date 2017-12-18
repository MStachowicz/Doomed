using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;

namespace OpenGL_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : GameWindow
    {
        public const int WIDTH = 1600;
        public const int HEIGHT = 900;

        public Matrix4 projection;
        EntityManager entityManager;
        SystemManager systemManager;

        public static float dt;
        public Camera playerCamera;

        public static MyGame gameInstance;

        public static MazeLevel currentLevelLoaded;

        public MyGame() : base(WIDTH, HEIGHT)
        {
            gameInstance = this;

            playerCamera = new Camera();
            entityManager = new EntityManager();
            systemManager = new SystemManager();
        }
       
        private void CreateEntities()
        {
            Entity newEntity;

            newEntity = new Entity("Player");
            newEntity.AddComponent(new ComponentInput());
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentVelocity(0, 0, 0));
            newEntity.AddComponent(new ComponentScale(0, 0, 0));
            entityManager.AddEntity(newEntity);

            currentLevelLoaded = new Level1();

            for (int i = 0; i < currentLevelLoaded.wallPositions.Count; i++)
            {
                newEntity = new Entity("MazeWall");
                newEntity.AddComponent(new ComponentVelocity(0, 0, 0.0f));

                newEntity.AddComponent(new ComponentPosition(currentLevelLoaded.wallPositions[i]));
                newEntity.AddComponent(new ComponentRotation(currentLevelLoaded.wallRotations[i]));
                newEntity.AddComponent(new ComponentScale(currentLevelLoaded.wallScales[i])); 

                newEntity.AddComponent(new ComponentGeometry("Geometry/QuadGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/Oak.png"));
                entityManager.AddEntity(newEntity);
            }
        }

       private void Collision(Vector2 oldPosition, Vector2 newPosition)
        {
            foreach (MazeLevel.WallPoints w in currentLevelLoaded.wallPlanePositions)
            {
                float dx = w.endPosition.X - w.startPosition.X;
                float dy = w.endPosition.Y - w.startPosition.Y;
                //   Vector2 normal = new Vector2(-dy, dx).Normalized();
                Vector2 normal = new Vector2(16, 12.5f).Normalized();
            //  Vector2 normal = new Vector2(- w.startPosition.Y,w.startPosition.X).Normalized();



                float oldPos = dotProduct(normal, oldPosition);
                    float newPos = dotProduct(normal, newPosition);
             

                    if (newPos * oldPos < 0)
                    {
                  //  normal = new Vector2(-oldPosition.Y, oldPosition.X);
                 //    oldPos = dotProduct(normal, oldPosition);
                 //    newPos = dotProduct(normal, newPosition);
                //    if (oldPos + newPos < 0)
                 //   {
                        playerCamera.Position = new Vector3(oldPosition.X, 0, oldPosition.Y);

                 //   }

              }
            }
        }
            


        private float dotProduct(Vector2 vA, Vector2 vB)
        {
            float dot = (vA.X * vB.X) + (vA.Y * vB.Y);
            return dot;
        }

    

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemInput();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to setup the environment and matrices.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), WIDTH / HEIGHT, 0.01f, 100f);

            CreateEntities();
            CreateSystems();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            // ------------------------ TIMING ------------------------
            dt = (float)(e.Time);


            // TODO: Add your update logic here
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Vector2 oldPosition = new Vector2(playerCamera.Position.X, playerCamera.Position.Z);
            systemManager.ActionSystems(entityManager);
            Vector2 newPosition = new Vector2(playerCamera.Position.X, playerCamera.Position.Z);
            Collision(oldPosition, newPosition);

            GL.Flush();
            SwapBuffers();
        }

        /// <summary>
        /// Mouse is contained inside the GameWindow class.
        /// </summary>
        public static Vector2 GetMousePosition()
        {
            return new Vector2(gameInstance.Mouse.X, gameInstance.Mouse.Y);
        }

        public static MouseDevice GetMouse()
        {
            return gameInstance.Mouse;
        }

        public static void ExitGame()
        {
            gameInstance.Exit();
        }
    }
}
