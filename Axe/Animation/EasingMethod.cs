using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface EasingMethod
    {
        float motion(float d);

        String Name();
    }
}
