using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface Asset 
    {
         //AssetInfo getInfo();
	
	     //void setInfo(AssetInfo info);
	
	     bool isLoaded();
	
	     void load();

	     bool isActivated();
	
	     void activate();

        void delete();
    }
}
