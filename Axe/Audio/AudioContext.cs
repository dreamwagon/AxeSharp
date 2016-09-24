using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// An audio context dictates that a set of Audio objects are required for
    /// a certain state of the game. When loaded into the AudioManager it will
    /// ensure that all Audios specified in the context are loaded, and if they
    /// are not it will load them right there. The context can also be unloaded
    /// from the AudioManager which may cause Audio to be disposed if it is
    /// not referenced by any active context.
    /// </summary>
    public class AudioContext
    {
        private List<Audio> audioList = new List<Audio>();

        /// <summary>
        /// Instantiates a new AudioContext.
        /// </summary>
        /// <param name="audioArray">The set of Audio that exist in this Context.</param>
        public AudioContext(params Object[] audioArray)
        {
            foreach (Object obj in audioArray)
            {
                if ( obj is Audio )
                {
                    Audio a = (Audio)obj;

                    audioList.Add(a);
                }
                else if (obj is AudioBank)
                {
                    AudioBank b = (AudioBank)obj;

                    foreach (Audio a in b.Audio)
                    {
                        audioList.Add(a);
                    }
                }
                else if (obj is AudioQueue)
                {
                    AudioQueue q = (AudioQueue)obj;

                    foreach (Audio a in q.Audio)
                    {
                        audioList.Add(a);
                    }
                }
            }
        }

        /// <summary>
        /// The list of Audio in this Context.
        /// </summary>
        public List<Audio> Audio
        {
            get { return audioList; }
        }

    }

}
