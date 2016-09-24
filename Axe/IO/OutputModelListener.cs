using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface OutputModelListener<T>
    {
	    void onWrite(OutputModel parent, OutputModel child, T value);
    }
}
