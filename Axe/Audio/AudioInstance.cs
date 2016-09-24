using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// A delegate for listening to state changes in an AudioInstance.
    /// </summary>
    /// <param name="instance">The AudioInstance which has changed state.</param>
    /// <param name="newState">The new SoundState.</param>
    /// <param name="oldState">The old SoundState.</param>
    public delegate void AudioStateEvent(AudioInstance instance, SoundState newState, SoundState oldState);

    /// <summary>
    /// A playable instance of an Audio object. An AudioInstance can have:
    /// 1. A set of flags which are used for audio control at the highest level.
    /// 2. A current state.
    /// 3. A list of listeners to state change events.
    /// 4. A maximum length in milliseconds to play. If the instance is set to
    ///     loop it will loop continuously until this length is reached. However
    ///     if the length is 0 then the instance may loop indefinitely.
    /// </summary>
    public class AudioInstance
    {
        public event AudioStateEvent OnStateEvent;

        private int flags;
        private Audio audio;
        private SoundState state;
        private SoundEffectInstance instance;
        private int length;
        private int time;

        /// <summary>
        /// Instantiates a new AudioInstance with a length which matches the
        /// duration of the given Audio object.
        /// </summary>
        /// <param name="audio">The parent Audio object.</param>
        /// <param name="flags">The flags for this instance.</param>
        public AudioInstance(Audio audio, int flags)
            : this(audio, flags, 0)
        {
        }

        /// <summary>
        /// Instantiates a new AudioInstance.
        /// </summary>
        /// <param name="audio">The parent Audio object.</param>
        /// <param name="flags">The flags for this instance.</param>
        /// <param name="length">The maximum playing length of this instance
        /// in milliseconds or zero if indefinite.</param>
        public AudioInstance(Audio audio, int flags, int length)
        {
            this.audio = audio;
            this.flags = flags;
            this.length = length;
            this.state = SoundState.Stopped;

            if (audio.Effect != null && !audio.Effect.IsDisposed)
            {

                this.instance = audio.Effect.CreateInstance();

            }
            else
            {
                Console.WriteLine("Audio effect null or disposed: " + audio.AssetName);
            }
        }

        /// <summary>
        /// The reference to the parent Audio object.
        /// </summary>
        public Audio Audio
        {
            get { return audio; }
        }

        /// <summary>
        /// The SoundEffectInstance for this AudioInstance.
        /// </summary>
        public SoundEffectInstance Effect
        {
            get { return instance; }
        }

        /// <summary>
        /// The current state of the instance.
        /// </summary>
        public SoundState State
        {
            get { return state; }
        }

        /// <summary>
        /// Returns true when the state of the instance is stopped.
        /// </summary>
        public bool HasExpired
        {
            get { return state == SoundState.Stopped; }
        }

        /// <summary>
        /// The maximum play length for the instance in milliseconds, or zero
        /// if the instance should play indefinitely if it is loopable.
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = 0; }
        }

        /// <summary>
        /// How long the instance has been playing in milliseconds.
        /// </summary>
        public int Time
        {
            get { return time; }
        }

        /// <summary>
        /// The flags for this instance.
        /// </summary>
        public int Flags
        {
            get { return flags; }
        }

        /// <summary>
        /// Whether this instance has any of the given flags.
        /// </summary>
        /// <param name="inputFlags">The given flags.</param>
        /// <returns>True if this instance has any of the given flags, otherwise false.</returns>
        public bool HasFlags(int inputFlags)
        {
            return HasFlags(inputFlags, MatchType.AnyOf);
        }

        /// <summary>
        /// Whether this instance has any of the given flags based on a matching predicate.
        /// </summary>
        /// <param name="inputFlags">The given flags.</param>
        /// <param name="match">The matching predicate</param>
        /// <returns>True if this instance has matching flags, otherwise false.</returns>
        public bool HasFlags(int inputFlags, MatchType match)
        {
            return MatchTypeUtility.IsMatch(flags, inputFlags, match);
        }

        /// <summary>
        /// Updates this instance by keeping time and notifying any listeners of state changes.
        /// </summary>
        /// <param name="gameTime">The GameTime for the current frame.</param>
        public void Update(GameTime gameTime)
        {
            // If playing, update time.
            if (instance.State == SoundState.Playing)
            {
                if (length > 0)
                {
                    time += gameTime.ElapsedGameTime.Milliseconds;

                    if (time >= length)
                    {
                        instance.Stop(true);
                    }
                }
            }

            SoundState newState = instance.State;

            // If state has changed, notify listeners.
            if (newState != state && OnStateEvent != null)
            {
                OnStateEvent(this, newState, state);
            }

            state = newState;
        }
    }
}
