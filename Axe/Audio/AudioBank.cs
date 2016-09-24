   using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// An audio bank is a collection of Audio which have a weighted
    /// probability of being randomly chosen. 
    /// </summary>
    public class AudioBank : AudioInstanceFactory
    {
        private static Random random = new Random();

        private Audio[] audio;
        private int[] distribution;

        /// <summary>
        /// Instantiates a new AudioBank.
        /// </summary>
        /// <param name="probability">The probability map between Audio and a 
        /// weight which dictates the probability of occurring.</param>
        public AudioBank(Dictionary<Audio, int> probability)
        {
            Set(probability);
        }

        /// <summary>
        /// Sets the probability map.
        /// </summary>
        /// <param name="probability">The probability map between Audio and a 
        /// weight which dictates the probability of occurring.</param>
        public void Set(Dictionary<Audio, int> probability)
        {
            int count = 0, index = 0;

            foreach (KeyValuePair<Audio, int> entry in probability)
            {
                count += entry.Value;
            }

            audio = new Audio[probability.Count];
            distribution = new int[count];
            count = 0;

            foreach (KeyValuePair<Audio, int> entry in probability)
            {
                audio[index] = entry.Key;

                for (int i = 0; i < entry.Value; i++)
                {
                    distribution[count++] = index;
                }

                index++;
            }
        }

        /// <summary>
        /// Returns a random Audio from the bank.
        /// </summary>
        /// <returns>A random Audio selected based on the given weights.</returns>
        public Audio GetAudio()
        {
            return audio[distribution[random.Next(distribution.Length)]];
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

        /// <summary>
        /// Returns the distribution of the Audio in the bank.
        /// </summary>
        public int[] Distribution
        {
            get { return distribution; }
        }

    }

}
