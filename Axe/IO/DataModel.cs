using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface DataModel
    {
        void read(InputModel input);
        void write(OutputModel output);
    }
}
