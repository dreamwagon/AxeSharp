using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public abstract class AbstractEasingType : EasingType
    {
	    private String name;

	    public AbstractEasingType( String name )
	    {
		    this.name = name;
	    }

	    public String Name()
	    {
		    return name;
	    }

        public abstract float delta(float d, EasingMethod f);
    }
}
