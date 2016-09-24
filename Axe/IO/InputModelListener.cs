using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface InputModelListener
    {
	    Object onRead(InputModel parent, InputModel child);
    }
}
