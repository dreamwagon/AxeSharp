using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface Shape<S>
    {
	    S copy();
	    void copyTo( S outS );
	    void move(float t, Vec3f velocity);
    }
}
