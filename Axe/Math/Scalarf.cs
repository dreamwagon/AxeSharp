using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    /// <summary>
    /// An attribute for a single float value.
    /// </summary>
    public class Scalarf : Attribute<Scalarf>
    {
        public static float PI = (float)Math.PI;
        public static float PI2 = (float)(Math.PI * 2.0);

        public static Scalarf ZERO = new Scalarf(0f);
        public static Scalarf ONE = new Scalarf(1f);

        public float v;

        public Scalarf()
        {
        }

        public Scalarf(float v)
        {
            this.v = v;
        }

        public Scalarf(Scalarf v)
        {
            this.v = v.v;
        }

        public bool IsEqual(Scalarf value)
        {
            return (value.v == v);
        }

        public Scalarf Get()
        {
            return this;
        }

        public void Set(float value)
        {
            v = value;
        }

        public void Set(Scalarf value)
        {
            v = value.v;
        }

        public void Update(Scalarf target)
        {
            target.v = v;
        }

        public void Copy(Scalarf from, Scalarf to)
        {
            to.v = from.v;
        }

        public void Interpolate(Scalarf start, Scalarf end, float delta)
        {
            v = (end.v - start.v) * delta + start.v;
        }

        public void Add(Scalarf value, float delta)
        {
            v += value.v * delta;
        }
        public void Add(float s)
        {
            v += s;
        }
        public void Mul(Scalarf value)
        {
            v *= value.v;
        }

        public void Scale(float d)
        {
            v *= d;
        }

        public float Distance(Scalarf to)
        {
            return Math.Abs(v - to.v);
        }

        public Scalarf Clone()
        {
            return new Scalarf(v);
        }

        public Attribute<Scalarf> Create()
        {
            return new Scalarf();
        }

        public void Mod(float s)
        {
            v -= s * (float)Math.Floor(v / s);
        }

        public float Cos()
        {
            return (float)Math.Cos(v);
        }
        public float Sin()
        {
            return (float)Math.Sin(v);
        }

        public static float Cos(float angle)
        {
            return (float)Math.Cos(angle);
        }
        public static float Sin(float angle)
        {
            return (float)Math.Sin(angle);
        }

        public static Scalarf[] Array(params float[] values)
        {
            Scalarf[] s = new Scalarf[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                s[i] = new Scalarf(values[i]);
            }
            return s;
        }
    }
}
