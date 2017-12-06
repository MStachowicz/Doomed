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
        public const int WIDTH = 1900;
        public const int HEIGHT = 900;

        public Matrix4 projection;
        EntityManager entityManager;
        SystemManager systemManager;
        Vector3 emitterPosition;
        int myBuffer;
        int mySource;
        Vector3 listenerPosition;

        internal static byte[] LoadWave(FileStream fileStream, out int channels, out int bits, out int rate, out int chunkSize)
        {
            throw new NotImplementedException();
        }

        Vector3 listenerDirection;
        Vector3 listenerUp;
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
            AudioContext AC = new AudioContext();
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

            //LoadAudio();
        }

        private void LoadAudio()
        {
            // ********************************************************
            // ***                    Audio                         ***
            // ********************************************************

            // Setup OpenAL Listener
            listenerPosition = new Vector3(0, 0, 3);
            listenerDirection = new Vector3(0, 0, -1);
            listenerUp = Vector3.UnitY;

            // reserve a Handle for the audio file
            myBuffer = AL.GenBuffer();

            // Load a .wav file from disk.
            int channels, bits_per_sample, sample_rate;
            byte[] sound_data = LoadWave(
                File.Open("Audio/buzz.wav", FileMode.Open),
                out channels,
                out bits_per_sample,
                out sample_rate);
            ALFormat sound_format =
                channels == 1 && bits_per_sample == 8 ? ALFormat.Mono8 :
                channels == 1 && bits_per_sample == 16 ? ALFormat.Mono16 :
                channels == 2 && bits_per_sample == 8 ? ALFormat.Stereo8 :
                channels == 2 && bits_per_sample == 16 ? ALFormat.Stereo16 :
                (ALFormat)0; // unknown
            AL.BufferData(myBuffer, sound_format, sound_data, sound_data.Length, sample_rate);
            if (AL.GetError() != ALError.NoError)
            {
                // respond to load error etc.
            }

            // Create a sounds source using the audio clip
            mySource = AL.GenSource(); // gen a Source Handle
            AL.Source(mySource, ALSourcei.Buffer, myBuffer); // attach the buffer to a source
            AL.Source(mySource, ALSourceb.Looping, true); // source loops infinitely
            emitterPosition = new Vector3(10.0f, 0.0f, 0.0f);
            AL.Source(mySource, ALSource3f.Position, ref emitterPosition);
            AL.SourcePlay(mySource);
        }

        /// <summary>
        /// Load a WAV file.
        /// </summary>
        private static byte[] LoadWave(Stream stream, out int channels, out int bits, out int rate)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // RIFF header
                string signature = new string(reader.ReadChars(4));
                if (signature != "RIFF")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                int riff_chunck_size = reader.ReadInt32();

                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new NotSupportedException("Specified stream is not a wave file.");

                // WAVE header
                string format_signature = new string(reader.ReadChars(4));
                if (format_signature != "fmt ")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int format_chunk_size = reader.ReadInt32();
                int audio_format = reader.ReadInt16();
                int num_channels = reader.ReadInt16();
                int sample_rate = reader.ReadInt32();
                int byte_rate = reader.ReadInt32();
                int block_align = reader.ReadInt16();
                int bits_per_sample = reader.ReadInt16();

                string data_signature = new string(reader.ReadChars(4));
                if (data_signature != "data")
                    throw new NotSupportedException("Specified wave file is not supported.");

                int data_chunk_size = reader.ReadInt32();

                channels = num_channels;
                bits = bits_per_sample;
                rate = sample_rate;

                return reader.ReadBytes((int)reader.BaseStream.Length);
            }

        }
        public static ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1:
                    return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2:
                    return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default:
                    throw new NotSupportedException("The specified sound format is not supported.");
            }
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

            // ------------------------ AUDIO ------------------------
            // Move sounds source from right to left at 2.5 meters per second
            emitterPosition = new Vector3(emitterPosition.X - (float)(2.5 * e.Time), emitterPosition.Y, emitterPosition.Z);
            AL.Source(mySource, ALSource3f.Position, ref emitterPosition);

            // update OpenAL
            AL.Listener(ALListener3f.Position, ref listenerPosition);
            AL.Listener(ALListenerfv.Orientation, ref listenerDirection, ref listenerUp);

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
