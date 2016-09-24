using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public abstract class AbstractEasingMethod : EasingMethod
    {

	    private String name;

	    public AbstractEasingMethod( String name )
	    {
		    this.name = name;
	    }

	    public String Name()
	    {
		    return name;
	    }

        public abstract float motion(float d);

    }
}
