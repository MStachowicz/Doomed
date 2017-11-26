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
        public Matrix4 projection;
        EntityManager entityManager;
        SystemManager systemManager;
        Vector3 emitterPosition;
        int myBuffer;
        int mySource;
        Vector3 listenerPosition;
        Vector3 listenerDirection;
        Vector3 listenerUp;
        public static float dt;
        public Camera playerCamera;
        Vector2 lastMousePosition;
        bool firstMouse = true;
        int width = 1024, height = 768;

        public static MyGame gameInstance;

        public MyGame() : base(1024, 768)
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
            entityManager.AddEntity(newEntity);

            newEntity = new Entity("TestCube1");
            newEntity.AddComponent(new ComponentPosition(0.0f, 0.0f, 0.0f));
            newEntity.AddComponent(new ComponentGeometry("Geometry/CubeGeometry.txt"));
            newEntity.AddComponent(new ComponentTexture("Textures/Oak.png"));
            newEntity.AddComponent(new ComponentVelocity(0, 0, 0));
            entityManager.AddEntity(newEntity);
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

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), gameInstance.width / gameInstance.height, 0.01f, 100f);

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

            // ------------------------ INPUT ------------------------
            // Temporary way to move the camera using the mouse
            processMouseMove();

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Key.Escape))
                Exit();

            // ------------------------ AUDIO ------------------------
            // Move sounds source from right to left at 2.5 meters per second
            emitterPosition = new Vector3(emitterPosition.X - (float)(2.5 * e.Time), emitterPosition.Y, emitterPosition.Z);
            AL.Source(mySource, ALSource3f.Position, ref emitterPosition);

            // update OpenAL
            AL.Listener(ALListener3f.Position, ref listenerPosition);
            AL.Listener(ALListenerfv.Orientation, ref listenerDirection, ref listenerUp);

            // TODO: Add your update logic here
        }

        private void processMouseMove()
        {
            if (firstMouse) // prevents the screen jumping on first mouse lock to screen.
            {
                lastMousePosition.X = width / 2;
                lastMousePosition.Y = height / 2;
                firstMouse = false;
            }

            float xOffset = Mouse.X - lastMousePosition.X;
            float yOffset = lastMousePosition.Y - Mouse.Y;

            lastMousePosition.X = Mouse.X;
            lastMousePosition.Y = Mouse.Y;

            playerCamera.ProcessMouseMovement(xOffset, yOffset);
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

            systemManager.ActionSystems(entityManager);

            GL.Flush();
            SwapBuffers();
        }
        // Event based mouse camera movement, cant get this working at this time.
        void OnMouseMovement(object sender, MouseMoveEventArgs e)
        {
            playerCamera.ProcessMouseMovement(e.XDelta, e.YDelta);
        }
    }
}
