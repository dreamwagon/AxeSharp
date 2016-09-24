using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface EasingMethod
    {
        public float motion(float d);

        public String name();
    }
}
