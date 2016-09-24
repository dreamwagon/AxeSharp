using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    public class Frame
    {
        public Frame(Vector2 framePosition, float duration, int flags)
        {
            this.framePosition = framePosition;
            this.duration = duration;
            this.flags = flags;
        }

        public Frame(Vector2 framePosition, float duration, int flags, float rotation)
        {
            this.framePosition = framePosition;
            this.duration = duration;
            this.flags = flags;
            this.rotation = rotation;
        }

        public Frame(Vector2 framePosition, float duration, int flags, float rotation, float scale)
        {
            this.framePosition = framePosition;
            this.duration = duration;
            this.flags = flags;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Frame(Vector2 framePosition, float duration, int flags, float rotation, float scale, SpriteEffects effects)
        {
            this.framePosition = framePosition;
            this.duration = duration;
            this.flags = flags;
            this.rotation = rotation;
            this.scale = scale;
            this.effects = effects;
        }

        public bool HasFlags(int inputFlags)
        {
            return HasFlags(inputFlags, MatchType.All);
        }

        public bool HasFlags(int inputFlags, MatchType match)
        {
            return MatchTypeUtility.IsMatch(flags, inputFlags, match);
        }

        private Vector2 framePosition;
        public Vector2 FramePosition
        {
            get { return framePosition; }
            set { framePosition = value; }
        }

        private float duration;
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private float rotation = 0;
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }

        }
        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        private SpriteEffects effects = SpriteEffects.None;
        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }
        private int flags = 0;
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }
    }
}
