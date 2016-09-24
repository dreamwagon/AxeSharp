using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace com.dreamwagon.axe
{
    public class Animation
    {
        #region Constructors

        public Animation(Texture2D animationTexture, Rectangle clipRectangle, Vector2 origin, List<Frame> frameList, int repeatCount)
        {
            this.animationTexture = animationTexture;
            this.clipRectangle = clipRectangle;
            this.origin = origin;
            this.frameList = frameList;
            this.updateFlagsAndDuration();
            this.repeatCount = repeatCount;
        }

        public Animation(Texture2D animationTexture, Vector2[] framePositions, int[] durations, int[] flags, Rectangle clipRectangle, Vector2 origin, int repeatCount)
            : this(animationTexture, clipRectangle, origin, new List<Frame>(), repeatCount)
        {
            this.checkLengths(framePositions.Length, durations.Length, flags.Length);

            for (int j = 0; j < framePositions.Length; j++)
            {
                frameList.Add(new Frame(framePositions[j], durations[j], flags[j]));
            }

            this.updateFlagsAndDuration();
        }

        public Animation(Texture2D animationTexture, Vector2[] framePositions, int[] durations, int[] flags, float[] rotations, Rectangle clipRectangle, Vector2 origin, int repeatCount)
            : this(animationTexture, clipRectangle, origin, new List<Frame>(), repeatCount)
        {
            this.checkLengths(framePositions.Length, durations.Length, flags.Length, rotations.Length);

            for (int j = 0; j < framePositions.Length; j++)
            {
                frameList.Add(new Frame(framePositions[j], durations[j], flags[j], rotations[j]));
            }

            this.updateFlagsAndDuration();
        }

        public Animation(Texture2D animationTexture, Vector2[] framePositions, int[] durations, int[] flags, float[] rotations, float[] scales, Rectangle clipRectangle, Vector2 origin, int repeatCount)
            : this(animationTexture, clipRectangle, origin, new List<Frame>(), repeatCount)
        {
            this.checkLengths(framePositions.Length, durations.Length, flags.Length, rotations.Length, scales.Length);

            for (int j = 0; j < framePositions.Length; j++)
            {
                frameList.Add(new Frame(framePositions[j], durations[j], flags[j], rotations[j], scales[j]));
            }

            this.updateFlagsAndDuration();
        }

        public Animation(Texture2D animationTexture, Vector2[] framePositions, int[] durations, int[] flags, float[] rotations, float[] scales, SpriteEffects[] effects, Rectangle clipRectangle, Vector2 origin, int repeatCount)
            : this(animationTexture, clipRectangle, origin, new List<Frame>(), repeatCount)
        {
            this.checkLengths(framePositions.Length, durations.Length, flags.Length, rotations.Length, scales.Length, effects.Length);

            for (int j = 0; j < framePositions.Length; j++)
            {
                frameList.Add(new Frame(framePositions[j], durations[j], flags[j], rotations[j], scales[j], effects[j]));
            }

            this.updateFlagsAndDuration();
        }

        private void checkLengths(params int[] lengths)
        {
            for (int i = 0; i < lengths.Length - 1; i++)
            {
                if (lengths[i] != lengths[i + 1])
                {
                    throw new Exception("Array " + i + " length is not equal to array " + (i + 1) + " length");
                }
            }
        }

        private void updateFlagsAndDuration()
        {
            duration = flags = 0;

            for (int j = 0; j < frameList.Count; j++)
            {
                duration += frameList[j].Duration;
                flags |= frameList[j].Flags;
            }
        }

        #endregion

        #region Methods
        private Texture2D animationTexture;
        public Texture2D AnimationTexture
        {
            get { return animationTexture; }
            set { animationTexture = value; }
        }

        private Rectangle clipRectangle;
        public Rectangle ClipRectangle
        {
            get { return clipRectangle; }
            set { clipRectangle = value; }
        }

        private Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        private bool animationComplete;
        public bool AnimationComplete
        {
            get { return animationComplete; }
            set { animationComplete = value; }
        }

        private int frameCounter = 0;
        public int FrameCounter
        {
            get { return frameCounter; }
            set { frameCounter = value; }
        }

        //0 is forever
        private int repeatCounter = 0;
        private int repeatCount = 0;
        public int RepeatCount
        {
            get { return repeatCount; }
            set { repeatCount = value; }
        }

        private List<Frame> frameList;
        public List<Frame> FrameList
        {
            get { return frameList; }
        }

        private float frameTime = 0;
        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = value; }
        }

        private float animationTime = 0;
        public float AnimationTime
        {
            get { return animationTime; }
            set { animationTime = value; }
        }

        public Frame CurrentFrame
        {
            get { return frameList.ElementAt( frameCounter ); }
        }

        public int FrameCount
        {
            get { return frameList == null ? 0 : frameList.Count; }
        }

        private float duration;
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private int flags;
        public int Flags
        {
            get { return flags; }
        }

        public bool HasFlags(int inputFlags)
        {
            return HasFlags(inputFlags, MatchType.All);
        }

        public bool HasFlags(int inputFlags, MatchType match)
        {
            return MatchTypeUtility.IsMatch(flags, inputFlags, match);
        }

        public void Reset()
        {
            animationComplete = false;
            frameTime = 0;
            animationTime = 0;
            frameCounter = 0;
        }

        public void Skip()
        {
            animationTime += CurrentFrame.Duration - frameTime;
            frameTime = 0;

            NextFrame();
        }

        public void Update( GameTime gameTime )
        {
            frameTime += gameTime.ElapsedGameTime.Milliseconds;
            animationTime += gameTime.ElapsedGameTime.Milliseconds;

            if (frameTime > CurrentFrame.Duration )
            {
                frameTime -= CurrentFrame.Duration;

                NextFrame();
            }
        }

        private void NextFrame()
        {
            frameCounter++;

            if (frameCounter >= frameList.Count)
            {
                frameCounter = 0;
                animationTime = 0;

                if (repeatCount != 0)
                {
                    repeatCounter++;

                    if (repeatCounter >= repeatCount)
                    {
                        animationComplete = true;
                    }
                }
            }
            clipRectangle.X = (int)(clipRectangle.Width * CurrentFrame.FramePosition.X);
            clipRectangle.Y = (int)(clipRectangle.Height * CurrentFrame.FramePosition.Y);
        }

        #endregion
    }
}
