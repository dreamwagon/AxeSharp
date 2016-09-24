using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    /// <summary>
    /// Utility class for math functions.
    /// </summary>
    public class Numbers
    {
        public static float PI = 3.14159265358979323846f;
	    public static float PI2 = PI * 2;
        public static float DEGREE_TO_RADIAN = PI / 180;
	    public static float RADIAN_TO_DEGREE = 180 / PI;

        public static float[] RADIAN;
	    public static float[] COS;
	    public static float[] SIN;

        private static Random rnd = new Random();

        public static float Clamp(float v, float min, float max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        public static byte Clamp(byte v, byte min, byte max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        public static short Clamp(short v, short min, short max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        public static int Clamp(int v, int min, int max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        public static int Choose(int n, int m)
        {
            int num = 1, den = 1, gcd;

            if (m > (n >> 1))
            {
                m = n - m;
            }

            while (m >= 1)
            {
                num *= n--;
                den *= m--;
                gcd = Numbers.Gcd(num, den);
                num /= gcd;
                den /= gcd;
            }

            return num;
        }

        public static int Gcd(int a, int b)
        {
            int shift = 0;

            if (a == 0 || b == 0)
            {
                return (a | b);
            }

            for (shift = 0; ((a | b) & 1) == 0; ++shift)
            {
                a >>= 1;
                b >>= 1;
            }

            while ((a & 1) == 0)
            {
                a >>= 1;
            }

            do
            {
                while ((b & 1) == 0)
                {
                    b >>= 1;
                }
                if (a < b)
                {
                    b -= a;
                }
                else
                {
                    int d = a - b;
                    a = b;
                    b = d;
                }
                b >>= 1;
            } while (b != 0);

            return (a << shift);
        }

        public static int RandomSign()
        {
            return rnd.Next(3) - 1;
        }

        public static float Random(float max)
        {
            return (float)(max * rnd.NextDouble());
        }

        public static float Random(float min, float max)
        {
            return (float)((max - min) * rnd.NextDouble() + min);
        }

        public static int Random(int max)
        {
            return rnd.Next(max);
        }

        public static int Random(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        public static T Random<T>(T[] values, T defaultValue)
        {
            if (values == null || values.Length == 0)
            {
                return defaultValue;
            }

            return values[rnd.Next(values.Length)];
        }

        public static float EPSILON = 0.00001f;

        public static bool equals(float a, float b)
        {
            return equals(a, b, EPSILON);
        }

        public static bool equals(float a, float b, float epsilon)
        {
            return Math.Abs(a - b) < epsilon;
        }
    }
}
