using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
 
    public class Function
    {
        /// <summary>
        /// Delegate methods
        /// </summary>

        public delegate float MethodDelegate(float d);
        public delegate float TypeDelegate( float d, MethodDelegate f );
    
        /// <summary>
        /// Types
        /// </summary>

        public static float In(float d, MethodDelegate f)
        {
            return f(d);
        }
        public static float Out(float d, MethodDelegate f)
        {
            return (1 - f(1 - d));
        }
        public static float InOut(float d, MethodDelegate f)
        {
            return (d < 0.5 ? (f(2 * d) * 0.5f) : (1 - f(2 - 2 * d) * 0.5f));
        }
        public static float PingPong(float d, MethodDelegate f)
        {
            return (d < 0.5 ? (f(2 * d)) : (f(2 - 2 * d)));
        }

        /// <summary>
        /// Methods
        /// </summary>

        public static float Linear(float d)
        {
            return d;
        }
        public static float Quadratic(float d)
        {
            return d * d;
        }
        public static float Cubic(float d)
        {
            return d * d * d;
        }
        public static float Quartic(float d)
        {
            return d * d * d * d;
        }
        public static float Quintic(float d)
        {
            return d * d * d * d * d;
        }
        public static float Back(float d)
        {
            float d2 = d * d;
            float d3 = d2 * d;
            return d3 + d2 - d;
        }
        public static float Sine(float d)
        {
            const double FREQUENCY = Math.PI * 0.5;
            return (float)Math.Sin(d * FREQUENCY);
        }
        public static float Elastic(float d)
        {
            const double FREQUENCY = Math.PI * 3.5;
            float d2 = d * d;
            float d3 = d2 * d;
            float scale = d2 * ((2 * d3) + d2 - (4 * d) + 2);
            float wave = -(float)Math.Sin(d * FREQUENCY);
            return scale * wave;
        }
        public static float Revisit(float d)
        {
            const double FREQUENCY = Math.PI;
            return (float)Math.Abs(-Math.Sin(d * FREQUENCY) + d);
        }
        public static float SlowBounce(float d)
        {
            const double FREQUENCY = Math.PI * Math.PI * 1.5;
            float d2 = d * d;
            return (float)(1 - Math.Abs((1 - d2) * Math.Cos(d2 * d * FREQUENCY)));
        }
        public static float Bounce(float d)
        {
            const double FREQUENCY = Math.PI * Math.PI * 1.5;
            return (float)(1 - Math.Abs((1 - d) * Math.Cos(d * d * FREQUENCY)));
        }
        public static float SmallBounce(float d)
        {
            const double FREQUENCY = Math.PI * Math.PI * 1.5;
            float inv = 1 - d;
            return (float)(1 - Math.Abs(inv * inv * Math.Cos(d * d * FREQUENCY)));
        }
        public static float TinyBounce(float d)
        {
            const double FREQUENCY = 7;
            float inv = 1 - d;
            return (float)(1 - Math.Abs(inv * inv * Math.Cos(d * d * FREQUENCY)));
        }
        public static float Hesistant(float d)
        {
            return (float)(Math.Cos(d * d * 12) * d * (1 - d) + d);
        }
        public static float Lasso(float d)
        {
            float d2 = d * d;
            return (float)(1 - Math.Cos(d2 * d * 36) * (1 - d));
        }
        public static float Sqrt(float d)
        {
            return (float)Math.Sqrt(d);
        }
        public static float Log10(float d)
        {
            return (float)Math.Log10(d);
        }
        public static float Slingshot(float d)
        {
            if (d < 0.7f) return (d * -0.357f);
            float x = (d - 0.7f);
            return ((x * x * 27.5f - 0.5f) * 0.5f);
        }

        private TypeDelegate type;
        private MethodDelegate method;
        private float scale;

        public Function() 
            : this(In, Linear, 1f)
        {
        }

        public Function(TypeDelegate type)
            : this(type, Linear, 1f)
        {
        }

        public Function(MethodDelegate method)
            : this(In, method, 1f)
        {
        }

        public Function(TypeDelegate type, MethodDelegate method)
            : this(type, method, 1f)
        {
        }

        public Function(TypeDelegate type, MethodDelegate method, float scale)
        {
            this.type = type;
            this.method = method;
            this.scale = scale;
        }

        public float Delta(float delta)
        {
            float d = type(delta, method);

            if (scale != 1f)
            {
                d = scale * d + (1 - scale) * delta;
            }

            return d;
        }

        public TypeDelegate Type
        {
            get { return type; }
            set { type = value; }
        }

        public MethodDelegate Method
        {
            get { return method; }
            set { method = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

    }

}
