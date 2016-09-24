   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// An audio queue is a collection of Audio which have an equal
    /// probability of being chosen, and will never chose an audio
    /// twice before all audios have been chosen. This rule also applies
    /// to the third, forth, etc.
    /// </summary>
    public class AudioQueue : AudioInstanceFactory
    {
        private static Random random = new Random();

        private Audio[] audio;
        private int[] pointers;
        private int index;

        /// <summary>
        /// Instantiates a new AudioQueue.
        /// </summary>
        /// <param name="audio">The array of audio in the Queue.</param>
        public AudioQueue(params Audio[] audio)
        {
            this.audio = audio;
            this.index = -1;
            this.pointers = new int[audio.Length];

            for (int i = 0; i < audio.Length; i++)
            {
                pointers[i] = i;
            }
        }

        /// <summary>
        /// Returns an Audio from the Queue.
        /// </summary>
        /// <returns>A random Audio selected based on the given weights.</returns>
        public Audio GetAudio()
        {
            index = (index + 1) % audio.Length;

            if (index == 0)
            {
                for (int i = 0; i < audio.Length; i++)
                {
                    int k = random.Next(audio.Length);
                    int t = pointers[i];
                    pointers[i] = pointers[k];
                    pointers[k] = t;
                }
            }

            return audio[pointers[index]];
        }

        /// <summary>
        /// Creates a new AudioInstance.
        /// </summary>
        /// <param name="flags">The flags the AudioInstance should have. This 
        /// is used to apply volume, pausing, resumes, pans, and several other 
        /// sound effect changes in one command to many playing effects.</param>
        /// <returns>A new AudioInstance. The AudioInstance must be tracked
        /// by the AudioManager once its played.</returns>
        public AudioInstance NewInstance(int flags)
        {
            return GetAudio().NewInstance(flags);
        }

        /// <summary>
        /// Returns the array of Audio that exist in this bank.
        /// </summary>
        public Audio[] Audio
        {
            get { return audio; }
        }

    }

}
