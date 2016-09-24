using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace com.dreamwagon.axe
{
    public class Tween
    {
        private float start;
        private float end;
        private float value;
        private float time;
        private float duration;
        private Function function;
        private bool repeat = false;

        public Tween(float start, float end, float duration)
            : this( start, end, duration, new Function(), false )
        {
        }

        public Tween(float start, float end, float duration, bool repeat)
            : this(start, end, duration, new Function(), repeat)
        {
        }
        public Tween(float start, float end, float duration, Function function)
            : this(start, end, duration, function, false)
        {
        }
        public Tween(float start, float end, float duration, Function function, bool repeat)
        {
            this.repeat = repeat;
            this.start = start;
            this.end = end;
            this.duration = duration;
            this.function = function;
            this.Reset();
        }

        public void Reset()
        {
            this.value = start;
            this.time = 0;
        }

        public void Update(GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds * 0.001f;

            time += dt;

            if (time > duration)
            {
                time = duration;
            }

            value = (end - start) * function.Delta(time / duration) + start;

            if (repeat)
            {
                if (IsComplete())
                {
                    Reset();
                }
            }
        }

        public bool IsComplete()
        {
            return (time == duration);
        }

        public float Start
        {
            get { return start; }
            set { start = value; }
        }

        public float End
        {
            get { return end; }
            set { end = value; }
        }

        public Function Function
        {
            get { return function; }
            set { function = value; }
        }

        public float Time
        {
            get { return time; }
        }

        public float Duration
        {
            get { return duration; }
            set {  duration = value; }
        }

        public float Value
        {
            get { return value; }
        }

        public void MakeComplete()
        {
            time = duration;
        }

    }
}
