using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Audio.OpenAL;


namespace OpenGL_Game.Systems
{

    public class SystemAudio
    {
        private int bits;
        private int channels;
        private int rate;
        private byte[] soundData;
        private int buffer;
        private int source;

        /// <summary>
        /// Constructor method for sounds
        /// </summary>
        /// <param name="activebuffer"></param>
        /// <param name="filename"></param>
        public SystemAudio(string filename)
        {
            buffer = AL.GenBuffer();
            source = AL.GenSource();
            int chunkSize;
            soundData = MyGame.LoadWave(File.Open(filename, FileMode.Open), out channels, out bits, out rate, out chunkSize);
            AL.BufferData(buffer, MyGame.GetSoundFormat(channels, bits), soundData, chunkSize, rate);
            Console.WriteLine(MyGame.GetSoundFormat(channels, bits).ToString());

            ALError error = AL.GetError();
            if (error != ALError.NoError)
            {
                Console.WriteLine("error loading buffer: " + error);
            }

            AL.Source(source, ALSourcei.Buffer, buffer);
            var sourcePosition = new Vector3(0f, 0f, 0f);
            AL.Source(source, ALSource3f.Position, ref sourcePosition);
            AL.Source(source, ALSourcef.Gain, 0.85f);
            var listenerPosition = new Vector3(0, 0, 0);
            AL.Listener(ALListener3f.Position, ref listenerPosition);
        }

        public bool IsPlaying()
        {
            ALSourceState state = AL.GetSourceState(source);
            return state == ALSourceState.Playing;
        }

        public bool IsPaused()
        {
            ALSourceState state = AL.GetSourceState(source);
            return state == ALSourceState.Paused;
        }

        public bool IsStopped()
        {
            ALSourceState state = AL.GetSourceState(source);
            return state == ALSourceState.Stopped;
        }

        public void Play()
        {
            Play(false);
        }

        public void Play(bool loop)
        {
            AL.Source(source, ALSourceb.Looping, loop);
            AL.SourcePlay(source);
        }

        public void Pause()
        {
            AL.SourcePause(source);
        }

        public void Stop()
        {
            AL.SourceStop(source);
        }
    }
}