using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class Bitset
    {
	    public long x = 0;
	
	    public void clear() 
	    {
		    x = 0;
	    }
	
	    public void setIndex(int i, bool on) 
	    {
		    if (on) 
		    {
			    x |= (1L << i);
		    }
		    else 
		    {
			    x &= ~(1L << i);
		    }
	    }
	
	   /* public void set<E>(E enumConstant, bool on)
	    {
		    if (on) 
		    {
			    x |= (1L << enumConstant.ordinal());
		    }
		    else 
		    {
			    x &= ~(1L << enumConstant.ordinal());
		    }
	    }*/
	
	    public void add(long y) 
	    {
		    x |= y;
	    }
	
	    public void addIf(long y, bool condition) 
	    {
		    if (condition) 
		    {
			    x |= y;
		    }
	    }
	
	    /*public <E extends Enum<E>> void add(E enumConstant) 
	    {
		    x |= (1L << enumConstant.ordinal());
	    }
	
	    public <E extends Enum<E>> void addIf(E enumConstant, bool condition) 
	    {
		    if (condition) 
		    {
			    x |= (1L << enumConstant.ordinal());
		    }
	    }*/
	
	    public void addIndex(int i) 
	    {
		    x |= (1L << i);
	    }
	
	    public void addIndexIf(int i, bool condition) 
	    {
		    if (condition) 
		    {
			    x |= (1L << i);
		    }
	    }
	
	    public void remove(long y) 
	    {
		    x &= ~y;
	    }
	
	    public void removeIf(long y, bool condition) 
	    {
		    if (condition) 
		    {
			    x &= ~y;
		    }
	    }
	
	   /* public <E extends Enum<E>> void remove(E enumConstant) 
	    {
		    x &= ~(1L << enumConstant.ordinal());
	    }
	
	    public <E extends Enum<E>> void removeIf(E enumConstant, bool condition) 
	    {
		    if (condition) 
		    {
			    x &= ~(1L << enumConstant.ordinal());
		    }
	    }*/
	
	    public void removeIndex(int i) 
	    {
		    x &= ~(1L << i);
	    }
	
	    public void removeIndexIf(int i, bool condition) 
	    {
		    if (condition) 
		    {
			    x &= ~(1L << i);
		    }
	    }
	
	    public void toggle(long y) 
	    {
		    x ^= y;
	    }
	
	    public void toggleIf(long y, bool condition) 
	    {
		    if (condition) 
		    {
			    x ^= y;
		    }
	    }
	
	    /*public <E extends Enum<E>> void toggle(E enumConstant) 
	    {
		    x ^= (1L << enumConstant.ordinal());
	    }
	
	    public <E extends Enum<E>> void toggleIf(E enumConstant, bool condition) 
	    {
		    if (condition) 
		    {
			    x ^= (1L << enumConstant.ordinal());
		    }
	    }*/
	
	    public void toggleIndex(int i) 
	    {
		    x ^= (1L << i);
	    }
	
	    public void toggleIndexIf(int i, bool condition) 
	    {
		    if (condition) 
		    {
			    x ^= (1L << i);
		    }
	    }
	
	    public bool hasIndex(int i, MatchType match) 
	    {
		    return has(1L << i, match);
	    }
	
	    public bool matchesIndex(int i) 
	    {
		    return has(1L << i, MatchType.All);
	    }
	
	    public bool existsIndex(int i) 
	    {
		    return has(1L << i, MatchType.AnyOf);
	    }
	
	    public bool equalsIndex(int i) 
	    {
		    return has(1L << i, MatchType.Exact);
	    }
	
	    /*public <E extends Enum<E>> bool has(E enumConstant, MatchType match) 
	    {
		    return match.isMatch(x, 1L << enumConstant.ordinal());
	    }
	
	    public <E extends Enum<E>> bool matches(E enumConstant) 
	    {
		    return has(1L << enumConstant.ordinal(), Match.All);
	    }
	
	    public <E extends Enum<E>> bool exists(E enumConstant) 
	    {
		    return has(1L << enumConstant.ordinal(), MatchType.AnyOf);
	    }
	
	    public <E extends Enum<E>> bool equals(E enumConstant) 
	    {
		    return has(1L << enumConstant.ordinal(), MatchType.Exact);
	    }*/
	
	    public bool has(long y, MatchType match) 
	    {
            return MatchTypeUtility.IsMatch(x, y, match);
	    }
	
	    public bool matches(long y) 
	    {
		    return has(y, MatchType.All);
	    }
	
	    public bool exists(long y) 
	    {
		    return has(y, MatchType.AnyOf);
	    }
	
	    public bool equals(long y) 
	    {
		    return has(y, MatchType.Exact);
	    }
	
    }
}
