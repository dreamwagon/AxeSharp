using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe.Extensions
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random rnd, float min, float max)
        {
            return min + (float)rnd.NextDouble() * (max - min);
        }
    }
}
