using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// Audio is a reference to a resource which can be loaded as a 
    /// SoundEffect and has a duration in milliseconds. All Audios
    /// can be defined statically in a single class since they're not
    /// loaded when first instantiated (the InputManager handles
    /// loading the audio).
    /// </summary>
    public class Audio : AudioInstanceFactory
    {
        private string assetName;    
        private SoundEffect effect;

        private int duration;

        /// <summary>
        /// Instantiates a new Audio.
        /// </summary>
        /// <param name="resource">The Asset Name of the SoundEffect file.</param>
        /// <param name="duration">The duration of the SoundEffect in milliseconds.</param>
        public Audio(string resource, int duration)
        {
            this.assetName = resource;
            this.duration = duration;
        }
        
        /// <summary>
        /// The SoundEffect loaded for the Audio or null if the Audio is not 
        /// being used by the game currently.
        /// </summary>
        public SoundEffect Effect
        {
            get { return effect; }
        }

        /// <summary>
        /// Returns true if the effect is loaded (not null).
        /// </summary>
        public bool IsLoaded
        {
            get { return effect != null; }
        }

        /// <summary>
        /// Returns the Asset Name of the SoundEffect file.
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
        }

        /// <summary>
        /// Returns the Duration of the audio in milliseconds.
        /// </summary>
        public int Duration
        {
            get { return duration; }
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
            return new AudioInstance(this, flags);
        }

        /// <summary>
        /// Loads the effect if it isn't currently loaded.
        /// </summary>
        /// <param name="content">The ContentManager which can load the SoundEffect file.</param>
        public void Load(ContentManager content)
        {
            if (effect == null)
            {
                effect = content.Load<SoundEffect>(assetName);
            }
        }

        /// <summary>
        /// Unloads the effect if it has been loaded.
        /// </summary>
        public void Unload()
        {
            if (effect != null)
            {
                if (!effect.IsDisposed)
                {
                    effect.Dispose();
                }
                effect = null;
            }
        }

    }

}
