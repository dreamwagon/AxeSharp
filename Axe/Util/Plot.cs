using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface Plot<T>
    {
        bool next();
        void reset();
        void end();
        T get();
    }
}
