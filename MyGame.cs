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
        Vector3 listenerDirection;
        Vector3 listenerUp;
        public static float dt;
        public Camera playerCamera;

        public static MyGame gameInstance;

        public MyGame() : base(WIDTH, HEIGHT)
        {
            gameInstance = this;

            playerCamera = new Camera();
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            AudioContext AC = new AudioContext();
        }

        List<Vector3> wallPositions = new List<Vector3>();
        List<Vector3> wallRotations = new List<Vector3>();
        List<Vector3> wallScales = new List<Vector3>();
        void addWallPositions()
        {
            wallPositions.Add(new Vector3(1, 0, -2));
            wallPositions.Add(new Vector3(2, 0, -3));
            wallPositions.Add(new Vector3(2, 0, -4));
            wallPositions.Add(new Vector3(3, 0, -2.5f));
            wallPositions.Add(new Vector3(4.5f, 0, -4));
            wallPositions.Add(new Vector3(5.5f, 0, -2));
            wallPositions.Add(new Vector3(5, 0, -4.5f));
            wallPositions.Add(new Vector3(8.5f, 0, -3));
            wallPositions.Add(new Vector3(7, 0, -2));
            wallPositions.Add(new Vector3(9, 0, -1));
            wallPositions.Add(new Vector3(10.5f, 0, -2));
            wallPositions.Add(new Vector3(12, 0, -2));
            wallPositions.Add(new Vector3(2.5f, 0, -6));
            wallPositions.Add(new Vector3(2, 0, -7.5f));
            wallPositions.Add(new Vector3(8, 0, -5.5f));
            wallPositions.Add(new Vector3(9, 0, -7.5f));
            wallPositions.Add(new Vector3(7, 0, -7));
            wallPositions.Add(new Vector3(6, 0, -8));
            wallPositions.Add(new Vector3(5, 0, -9.5f));
            wallPositions.Add(new Vector3(2.5f, 0, -9));
            wallPositions.Add(new Vector3(2, 0, -10));
            wallPositions.Add(new Vector3(3, 0, -12));
            wallPositions.Add(new Vector3(6, 0, -10));
            wallPositions.Add(new Vector3(8, 0, -11));
            wallPositions.Add(new Vector3(10, 0, -4));
            wallPositions.Add(new Vector3(12, 0, -6));
            wallPositions.Add(new Vector3(11, 0, -8));
            wallPositions.Add(new Vector3(10.5f, 0, -9));
            wallPositions.Add(new Vector3(11, 0, -11));
            wallPositions.Add(new Vector3(7, 0, -10.5f));
            wallPositions.Add(new Vector3(10, 0, -15.5f));
            wallPositions.Add(new Vector3(6, 0, -13));
            wallPositions.Add(new Vector3(2, 0, -13));
            wallPositions.Add(new Vector3(1, 0, -14));
            wallPositions.Add(new Vector3(3, 0, -15));
            wallPositions.Add(new Vector3(6, 0, -16.5f));
            wallPositions.Add(new Vector3(8, 0, -14));
            wallPositions.Add(new Vector3(7.5f, 0, -16));
            wallPositions.Add(new Vector3(1, 0, -17));
            wallPositions.Add(new Vector3(2, 0, -18));
            wallPositions.Add(new Vector3(3.5f, 0, -17));
            wallPositions.Add(new Vector3(4, 0, -19));
            wallPositions.Add(new Vector3(8, 0, -18));
            wallPositions.Add(new Vector3(9, 0, -19));
            wallPositions.Add(new Vector3(5.5f, 0, -21));
            wallPositions.Add(new Vector3(1.5f, 0, -22));
            wallPositions.Add(new Vector3(2, 0, -22));
            wallPositions.Add(new Vector3(4.5f, 0, -22));
            wallPositions.Add(new Vector3(6, 0, -23));
            wallPositions.Add(new Vector3(8, 0, -21));
            wallPositions.Add(new Vector3(9.5f, 0, -21));
            wallPositions.Add(new Vector3(11, 0, -22));
            wallPositions.Add(new Vector3(12, 0, -21));
            wallPositions.Add(new Vector3(10.5f, 0, -20));
            wallPositions.Add(new Vector3(9, 0, -24));
            wallPositions.Add(new Vector3(11, 0, -23.5f));
            wallPositions.Add(new Vector3(12, 0, -10));
            wallPositions.Add(new Vector3(12, 0, -24.5f));
            wallPositions.Add(new Vector3(11, 0, -16));
            wallPositions.Add(new Vector3(11, 0, -19));
        }
        void addWallRotations()
        {
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
        }
        void addWallScales()
        {
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(7, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(5, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(5, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(1, 0, 1.25f));
            wallScales.Add(new Vector3(7, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(5, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(1, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(5, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(4, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(1, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(1, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
            wallScales.Add(new Vector3(2, 0, 1.25f));
        }

        /// <summary>
        /// Uses the maze walls hard coded at x below 12.5 to generate the remaining positions of the walls.
        /// </summary>
        void addSymmetricalWalls()
        {
            int wallsToCopy = wallPositions.Count;

            for (int i = 0; i < wallsToCopy; i++)
            {
                wallPositions.Add(new Vector3(25 - wallPositions[i].X, // line of symmetry lies at x = 12.5
                    0, wallPositions[i].Z));
                wallRotations.Add(new Vector3(wallRotations[i])); // repeat same rotations
                wallScales.Add(new Vector3(wallScales[i])); // repeat same scaling
            }
        }

        /// <summary>
        /// Adds all the walls which cross the line of symmetry.
        /// </summary>
        void addWallsCrossingSymmetry()
        {
            wallPositions.Add(new Vector3(12.5f, 0, -11));
            wallPositions.Add(new Vector3(12.5f, 0, -12));
            wallPositions.Add(new Vector3(12.5f, 0, -23));
            wallPositions.Add(new Vector3(12.5f, 0, -24));

            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallRotations.Add(new Vector3(90, 0, 0));

            wallScales.Add(new Vector3(1, 0, 1.25f));
            wallScales.Add(new Vector3(5, 0, 1.25f));
            wallScales.Add(new Vector3(3, 0, 1.25f));
            wallScales.Add(new Vector3(1, 0, 1.25f));
        }

        /// <summary>
        /// Adds the two doors in the starting area.
        /// </summary>
        void addDoors()
        {
            wallPositions.Add(new Vector3(12.5f, 1.25f, -16));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallScales.Add(new Vector3(1, 0, 1.25f));

            wallPositions.Add(new Vector3(12.5f, 1.25f, -19));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallScales.Add(new Vector3(1, 0, 1.25f));
        }

        /// <summary>
        /// Adds the walls surrounding the maze.
        /// </summary>
        void addOuterWalls()
        {
            // Left wall
            wallPositions.Add(new Vector3(0, 0, -12.5f));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallScales.Add(new Vector3(12.5f, 0.0f, 1.25f));
            // Right wall
            wallPositions.Add(new Vector3(25, 0, -12.5f));
            wallRotations.Add(new Vector3(90, 90, 0));
            wallScales.Add(new Vector3(12.5f, 0.0f, 1.25f));
            // Top wall
            wallPositions.Add(new Vector3(12.5f, 0, -25.0f));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallScales.Add(new Vector3(12.5f, 0.0f, 1.25f));
            // Bottom wall
            wallPositions.Add(new Vector3(12.5f, 0.0f, 0.0f));
            wallRotations.Add(new Vector3(90, 0, 0));
            wallScales.Add(new Vector3(12.5f, 0.0f, 1.25f));
        }

        /// <summary>
        /// Sets up all the information used to create the wall entities.
        /// </summary>
        void setupMazeEnvironment()
        {    
            // Maze walls
            addWallPositions();
            addWallRotations();
            addWallScales();
            addSymmetricalWalls();

            // Adding additional walls + doors
            addWallsCrossingSymmetry();
            addDoors();

            // Forgot to half the lengths of the walls (scaling is applied from center so need to scale by half)
            for (int i = 0; i < wallScales.Count; i++) // temporary fix (i need sleep..)
            {
                wallScales[i] = new Vector3(wallScales[i].X * 0.5f, wallScales[i].Y, wallScales[i].Z);
            }

            // Add the floor of the maze
            wallPositions.Add(new Vector3(12.5f, -1.0f, -12.5f));
            wallRotations.Add(new Vector3(0.0f, 0.0f, 0.0f));
            wallScales.Add(new Vector3(12.5f, 0.0f, 12.5f));

            addOuterWalls();
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
            
            setupMazeEnvironment();

            for (int i = 0; i < wallPositions.Count; i++)
            {
                newEntity = new Entity("MazeWall");
                newEntity.AddComponent(new ComponentVelocity(0, 0, 0.0f));

                newEntity.AddComponent(new ComponentPosition(wallPositions[i]));
                newEntity.AddComponent(new ComponentRotation(wallRotations[i]));
                newEntity.AddComponent(new ComponentScale(wallScales[i])); 

                newEntity.AddComponent(new ComponentGeometry("Geometry/QuadGeometry.txt"));
                newEntity.AddComponent(new ComponentTexture("Textures/Oak.png"));
                entityManager.AddEntity(newEntity);
            }
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

            systemManager.ActionSystems(entityManager);

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

        public static void ExitGame()
        {
            gameInstance.Exit();
        }
    }
}
