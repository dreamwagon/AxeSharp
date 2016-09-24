using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface EasingType
    {

        public float delta(float d, EasingMethod f);

        public String name();
    }
}
