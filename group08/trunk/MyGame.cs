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
    ///   /// <param name="e">Provides a snapshot of timing values.</param>
    public class MyGame : GameWindow
    {
        public const int WIDTH = 1600;
        public const int HEIGHT = 900;

        public Matrix4 projection;
        EntityManager entityManager;
        SystemManager systemManager;
        private float anim;
        static Vector2 oldCameraPosition;
        static Vector2 newCameraPosition;
        CubeMap skybox = new CubeMap();



        public static float dt;
        public static float dtt;
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
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            currentLevelLoaded = new Level1();
            currentLevelLoaded.loadEntities(entityManager);
            newEntity = new Entity("Drone");
        
            newEntity.AddComponent(new ComponentPosition(12.5f, 0.0f, -13.5f));
            newEntity.AddComponent(new ComponentRotation(0, 0, 0));
           newEntity.AddComponent(new ComponentScale(0.2f,0.2f,0.2f));
            newEntity.AddComponent(new ComponentAI());
            newEntity.AddComponent(new ComponentVelocity(0, 0, -0.2f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/cubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/Oak.png"));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Health");

            newEntity.AddComponent(new ComponentPosition(11f, -0.1f, -13.5f));
            newEntity.AddComponent(new ComponentRotation(0f, 90f, 0f));
            newEntity.AddComponent(new ComponentScale(0.1f, 0.2f, 0.1f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/cubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/heart.png"));
            newEntity.AddComponent(new ComponentPickUp(0, 50, 0));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Ammo");

            newEntity.AddComponent(new ComponentPosition(11f, -0.1f, -14.5f));
            newEntity.AddComponent(new ComponentRotation(0f, 90f, 0f));
            newEntity.AddComponent(new ComponentScale(0.1f, 0.25f, 0.1f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/cubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/Ammo.png"));
            newEntity.AddComponent(new ComponentPickUp(10, 0, 0));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("Drone_Dea");

            newEntity.AddComponent(new ComponentPosition(12f, -0.1f, -12.5f));
            newEntity.AddComponent(new ComponentRotation(0f, 90f, 0f));
            newEntity.AddComponent(new ComponentScale(0.1f, 0.2f, 0.1f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/cubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/robot.png"));
            //newEntity.AddComponent(new ComponentAudioEmitter("Audio/power_item_sound.wav"),);
            newEntity.AddComponent(new ComponentPickUp(0, 0, 5));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            // CREATING LIGHT ENTITIES
            Vector3 ambient = new Vector3(0.08f, 0.08f, 0.08f);
            Vector3 diffuse = new Vector3(0.1f, 0.1f, 0.1f);
            Vector3 specular = new Vector3(0.2f, 0.2f, 0.2f);

            newEntity = new Entity("pointLight");
            newEntity.AddComponent(new ComponentPosition(new Vector3(6.25f, 10.0f, -6.25f)));
            newEntity.AddComponent(new ComponentLightEmitter(ambient,diffuse ,specular));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("pointLight");
            newEntity.AddComponent(new ComponentPosition(new Vector3(6.25f, 10.0f, -18.75f)));
            newEntity.AddComponent(new ComponentLightEmitter(ambient, diffuse, specular));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("pointLight");
            newEntity.AddComponent(new ComponentPosition(new Vector3(18.75f, 10.0f, -6.25f)));
            newEntity.AddComponent(new ComponentLightEmitter(ambient, diffuse, specular));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("pointLight");
            newEntity.AddComponent(new ComponentPosition(new Vector3(18.75f, 10.0f, -18.75f)));
            newEntity.AddComponent(new ComponentLightEmitter(ambient, diffuse, specular));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("spotlight");
            newEntity.AddComponent(new ComponentPosition(playerCamera.Position));
            newEntity.AddComponent(new ComponentLightEmitter(
                new Vector3(0.0f, 0.0f, 0.0f),
                new Vector3(0.8f, 0.8f, 0.8f),
                new Vector3(1.0f, 1.0f, 1.0f)));
            newEntity.AddComponent(new ComponentLightDirection(playerCamera.Front, 12.5f, 17.5f));
            newEntity.AddComponent(new ComponentAlive());
            entityManager.AddEntity(newEntity);
        }

        private void Collision(Vector2 oldPosition, Vector2 newPosition)
        {
            foreach (MazeLevel.WallPoints w in currentLevelLoaded.wallPlanePositions)
            {

                float dx = w.endPosition.X - w.startPosition.X;
                float dy = w.endPosition.Y - w.startPosition.Y;

                Vector2 normal = new Vector2(-dy, dx);
                normal.Normalize();

                float oldPos = DotProduct(normal, oldPosition - w.startPosition);
                float newPos = DotProduct(normal, newPosition - w.startPosition);

                float q = (newPos * oldPos) - 0.01f;

                if (q < 0)
                {
                    dx = newPosition.X - oldPosition.X;
                    dy = newPosition.Y - oldPosition.Y;
                    normal = new Vector2(-dy, dx);

                    oldPos = DotProduct(normal, w.startPosition - oldPosition);
                    newPos = DotProduct(normal, w.endPosition - oldPosition);
                    float z = (newPos * oldPos) + 0.01f;
                    if ((newPos * oldPos) < 0)
                    {
                        if (w.startPosition.X == w.endPosition.X)
                        {
                            playerCamera.Position = new Vector3(oldPosition.X, 0, newPosition.Y);
                            playerCamera.Collision = true;
                        }
                        if (w.startPosition.Y == w.endPosition.Y)
                        {
                            playerCamera.Position = new Vector3(newPosition.X, 0, oldPosition.Y);
                            playerCamera.Collision = true;
                        }


                    }
                }
            }
        }
       
      
        

        public static float DotProduct(Vector2 vA, Vector2 vB)
        {
            float dot = (vA.X * vB.X) + (vA.Y * vB.Y);
            return dot;
        }


        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemLighting();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemInput();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAI();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPickUp();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAudio();
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
            this.CursorVisible = false;
            //GL.Enable(EnableCap.CullFace);


            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), WIDTH / HEIGHT, 0.01f, 100f);

            CreateEntities();
            CreateSystems();
            skybox.setupSkybox();
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

            oldCameraPosition = new Vector2(playerCamera.Position.X, playerCamera.Position.Z);
            systemManager.ActionSystems(entityManager);
            newCameraPosition = new Vector2(playerCamera.Position.X, playerCamera.Position.Z);
            Collision(oldCameraPosition, newCameraPosition);
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

            skybox.renderCubemap();

            systemManager.RenderSystems(entityManager);

            GL.Flush();
            SwapBuffers();
        }
        public static Vector2 GetOldCameraPosition()
        {
            return oldCameraPosition;
        }
        public static Vector2 GetNewCameraPosition()
        {
            return newCameraPosition;
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
