using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace com.dreamwagon.axe
{

    /// <summary>
    /// An AudioManager is a static class which manages loading and unloading 
    /// audio and keeping track of all playing Audio instances. Audio 
    /// loading/unloading is managed through AudioContexts. AudioContexts declare
    /// that the application requires these Audio files to be loaded and ready,
    /// if they aren't loaded already by another active context they're loaded
    /// immediately. AudioContexts can also be unmanaged which can unload
    /// an Audio file if no other active contexts are referencing it. Any
    /// AudioInstances created outside the manager must be tracked by the
    /// Track method, this will ensure all instances are updated and managed
    /// correctly.
    /// </summary>
    public class AudioManager : GameComponent
    {
        /// <summary>
        /// The maximum number of instances able to be played. Realistically no hardware
        /// could actually play this many anyway.
        /// </summary>
        public const int InstanceMax = 64;

        private static ContentManager content;
        private static Dictionary<Audio, int> audioReferences = new Dictionary<Audio, int>();
        private static List<AudioContext> contextList = new List<AudioContext>();
        private static AudioInstance[] instanceArray = new AudioInstance[InstanceMax];
        private static int instanceCount = 0;

        /// <summary>
        /// Instantiates a new AudioManager.
        /// </summary>
        /// <param name="game"></param>
        public AudioManager(Game game)
            : base(game)
        {
            content = game.Content;
        }

        /// <summary>
        /// An override Update method which is called every game update.
        /// </summary>
        /// <param name="gameTime">The GameTime object.</param>
        public override void Update(GameTime gameTime)
        {
            // How many instances are currently playing...
            int playing = 0;

            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                // Check that the Audio wasn't unloaded while it was being played...
                if (instance.Audio.IsLoaded)
                {
                    instance.Update(gameTime);
                }

                instanceArray[playing] = instance;

                // If the instance is still playing, and the audio hasn't been unloaded, 
                // consider it playing.
                if (!instance.HasExpired && instance.Audio.IsLoaded)
                {
                    playing++;
                }
                else
                {
                    instance.Effect.Dispose();
                }
            }

            // Remove any that aren't playing.
            while (instanceCount > playing)
            {
                instanceArray[--instanceCount] = null;
            }
        }


        /// <summary>
        /// An override Dispose method called when the Game stops.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            // Stop all playing instances
            while (--instanceCount >= 0)
            {
                instanceArray[instanceCount].Effect.Stop();
                instanceArray[instanceCount] = null;
            }

            // Unload all loaded Audio
            foreach (KeyValuePair<Audio, int> entry in audioReferences)
            {
                entry.Key.Unload();
            }

            // Clear context list and reference map.
            contextList.Clear();
            audioReferences.Clear();
        }

        /// <summary>
        /// Initializes this AudioManager by adding an instance of this 
        /// AudioManager to the game, and reference the content loader.
        /// </summary>
        /// <param name="game"></param>
        public static void Initialize(Game game)
        {
            game.Components.Add(new AudioManager(game));
        }


        /// <summary>
        /// Loads the given AudioContext. This will ensure that all audio in 
        /// the context is currently loaded and is ready for playing.
        /// </summary>
        /// <param name="context">The AudioContext to load.</param>
        public static void Load(AudioContext context)
        {
            // If it some how has the context already, ignore this call.
            if (contextList.Contains(context))
            {
                return;
            }

            contextList.Add(context);

            foreach (Audio a in context.Audio)
            {
                int references = 0;

                // Get existing reference count if any exist.
                if (audioReferences.ContainsKey(a))
                {
                    references = audioReferences[a];
                }
                else
                {
                    // Audio not referenced yet, load it.
                    a.Load(content);
                }

                references++;

                // Save new reference count.
                audioReferences[a] = references;
            }
        }

        /// <summary>
        /// Returns whether the given AudioContext is loaded already.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsLoaded(AudioContext context)
        {
            return contextList.Contains(context);
        }

        /// <summary>
        /// Unloads the given AudioContext. This will ensure that all audio
        /// in the context that is no longer referenced by any other context
        /// is disposed if specified.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="disposeUnreferenced">True if unreferenced Audio 
        /// should be disposed immediately. If it should not, Cleanup could 
        /// be called at a later time.</param>
        public static void Unload(AudioContext context, bool disposeUnreferenced)
        {
            // Only remove references if the context was loaded.
            if (contextList.Remove(context))
            {
                foreach (Audio a in context.Audio)
                {
                    int references = audioReferences[a];

                    references--;

                    // If nothing else references the Audio, remove it.
                    if (references == 0 && disposeUnreferenced)
                    {
                        audioReferences.Remove(a);

                        a.Unload();
                    }
                    else
                    {
                        audioReferences[a] = references;
                    }
                }
            }
        }

        /// <summary>
        /// Unloads any Audio that doesn't have any references.
        /// </summary>
        public static void Cleanup()
        {
            List<Audio> unreferenced = new List<Audio>();

            foreach (KeyValuePair<Audio, int> entry in audioReferences)
            {
                // No references you say?
                if (entry.Value == 0)
                {
                    unreferenced.Add(entry.Key);
                }
            }

            // Remove unreferenced Audio
            foreach (Audio a in unreferenced)
            {
                // Unload and remove I say!
                a.Unload();

                audioReferences.Remove(a);
            }
        }

        /// <summary>
        /// Keeps track of the AudioInstance in this manager and updates it. 
        /// This must be called for every AudioInstance thats created outside 
        /// of the Play method.
        /// </summary>
        /// <param name="instance"></param>
        public static void Track(AudioInstance instance)
        {
            instanceArray[instanceCount++] = instance; 
        }

        /// <summary>
        /// Given an AudioInstanceFactory (either Audio or AudioBank) and flags
        /// this will create an AudioInstance, play it, track it in the 
        /// AudioManager, and return it.
        /// </summary>
        /// <param name="factory">The factory used to create AudioInstances.</param>
        /// <param name="flags">The flags for the AudioInstance.</param>
        /// <returns>A new AudioInstance playing and managed.</returns>
        public static void Play(AudioInstanceFactory factory, int flags)
        {
             Play(factory, flags, false);
        }

        /// <summary>
        /// Given an AudioInstanceFactory (either Audio or AudioBank) and flags
        /// this will create an AudioInstance, play it, track it in the 
        /// AudioManager, and return it.
        /// </summary>
        /// <param name="factory">The factory used to create AudioInstances.</param>
        /// <param name="flags">The flags for the AudioInstance.</param>
        /// <returns>A new AudioInstance playing and managed.</returns>
        public static void Play(AudioInstanceFactory factory, int flags, bool loop)
        {
            // Only Play the instance if there's room... but there should always be room.
            if (instanceCount < InstanceMax)
            {
                AudioInstance instance = factory.NewInstance(flags);
                instance.Effect.IsLooped = loop;
                instance.Effect.Play();
                Track(instance);
            }
           /// return instance;
        }
        
        #region 3d sound
        static AudioEmitter  emitter = new AudioEmitter();
        static AudioListener listener = new AudioListener();
        public static void Play3d(Vector3 listenerPosition, Vector3 emitterPosition, 
                                  AudioInstanceFactory factory, int flags, bool loop)
        {
            // Only Play the instance if there's room... but there should always be room.
            if (instanceCount < InstanceMax)
            {
                listener.Position = listenerPosition;
                emitter.Position = emitterPosition;
                AudioInstance instance = factory.NewInstance(flags);
                instance.Effect.IsLooped = loop;
                instance.Effect.Apply3D(listener, emitter); 
                instance.Effect.Play();
                Track(instance);
            }
            /// return instance;
        }
        #endregion


        /// <summary>
        /// Given an AudioInstanceFactory (either Audio or AudioBank) and flags
        /// this will create an AudioInstance, play it, track it in the 
        /// AudioManager, and return it. If the current number of effects with the same
        /// flag exceed maxConcurrent, the effect will fail to play.
        /// </summary>
        /// <param name="factory">The factory used to create AudioInstances.</param>
        /// <param name="flags">The flags for the AudioInstance.</param>
        /// <param name="int">maxConcurrent number of effects to allow to be played.</param>
        /// <returns>A new AudioInstance playing and managed.</returns>
        public static void Play(AudioInstanceFactory factory, int flags, int maxConcurrent)
        {
            int flagCounter = 0;
            for (int k = 0; k < instanceCount; k++)
            {
                if (instanceArray[k].HasFlags(flags))
                {
                    flagCounter++;
                }
            }
            if (flagCounter < maxConcurrent)
            {
                Play(factory, flags, false);
            }
        }

        /// <summary>
        /// Returns a managed AudioInstance that has not been played yet. 
        /// This must be played before the next AudioManager update or this
        /// instance will automatically be removed. A reference to this instance
        /// should never be kept.
        /// </summary>
        /// <param name="factory">The factory used to create AudioInstances.</param>
        /// <param name="flags">The flags for the AudioInstance.</param>
        /// <returns>A new AudioInstance managed but not playing.</returns>
        public static AudioInstance Create(AudioInstanceFactory factory, int flags)
        {
            AudioInstance instance = factory.NewInstance(flags);
            Track(instance);
            return instance;
        }

        /// <summary>
        /// Sets the volume for all instances that have the given flags 
        /// according to the given match predicate.
        /// </summary>
        /// <param name="volume">The volume of the instances, between 0 and 1.</param>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void SetVolume(float volume, int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Volume = volume;
                }
            }
        }

        /// <summary>
        /// Sets the pan for all instances that have the given flags
        /// according to the given match predicate.
        /// </summary>
        /// <param name="pan">The pan of the instances, between -1(left) and 1(right)</param>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void SetPan(float pan, int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Pan = pan;
                }
            }
        }

        /// <summary>
        /// Sets the pitch for all instances that have the given flags
        /// according to the given match predicate.
        /// </summary>
        /// <param name="pan">The pitch of the instances, between -1 and 1.</param>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void SetPitch(float pitch, int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Pitch = pitch;
                }
            }
        }

        /// <summary>
        /// Pauses all instances that have the given flags according to the
        /// given match predicate.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void Pause(int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Pause();
                }
            }
        }

        /// <summary>
        /// Resumes all instances that have the given flags according to the
        /// given match predicate.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void Resume(int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Resume();
                }
            }
        }

        /// <summary>
        /// Stops all instances that have the given flags according to the
        /// given match predicate.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        public static void Stop(int flags, MatchType matchType)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    instance.Effect.Stop();
                }
            }
        }

        /// <summary>
        /// Retrieves all instances that have the given flags according to the
        /// given match predicate and adds them to the given collection. If 
        /// you want these instances removed from the AudioManager you can
        /// Stop them and they will automatically be removed.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match predicate.</param>
        /// <param name="destination">The collection to add the instances to.</param>
        public static void Retrieve(int flags, MatchType matchType, ICollection<AudioInstance> destination)
        {
            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    destination.Add(instance);
                }
            }
        }

        /// <summary>
        /// Counts the number of instances that have the given flags according
        /// to the given match predicate and the SoundState.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match criteria.</param>
        /// <param name="state">The SoundState they should have.</param>
        /// <returns>The number of matched instances.</returns>
        public static int Count(int flags, MatchType matchType, SoundState state)
        {
            int total = 0;

            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType) && (instance.State == state))
                {
                    total++;
                }
            }

            return total;
        }

        /// <summary>
        /// Counts the number of instances that have the given flags according
        /// to the given match predicate and the SoundState.
        /// </summary>
        /// <param name="flags">The input flags.</param>
        /// <param name="matchType">The match criteria.</param>
        /// <returns>The number of matched instances.</returns>
        public static int Count(int flags, MatchType matchType)
        {
            int total = 0;

            for (int i = 0; i < instanceCount; i++)
            {
                AudioInstance instance = instanceArray[i];

                if (instance.HasFlags(flags, matchType))
                {
                    total++;
                }
            }

            return total;
        }

    }
}
