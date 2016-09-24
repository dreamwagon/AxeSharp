using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class PlotCell3f : Plot<Vec3i>
    {
	    private Vec3f size = new Vec3f();
	    private Vec3f off = new Vec3f();
	    private Vec3f pos = new Vec3f();
	    private Vec3f dir = new Vec3f();

	    private Vec3i index = new Vec3i();
	
	    private Vec3f delta = new Vec3f();
	    private Vec3i sign = new Vec3i();
	    private Vec3f max = new Vec3f();
	
	    private int limit;
	    private int plotted;
	
	    public PlotCell3f(float offx, float offy, float offz, float width, float height, float depth)
	    {
		    off.Set( offx, offy, offz );
		    size.Set( width, height, depth );
	    }
	
	    public void plot(Vec3f position, Vec3f direction, int cells) 
	    {
		    limit = cells;
		
		    pos.Set( position );
		    dir.norm( direction );
		
		    delta.Set( size );
		    delta.Div( dir );
		
		    sign.x = (dir.x > 0) ? 1 : (dir.x < 0 ? -1 : 0);
		    sign.y = (dir.y > 0) ? 1 : (dir.y < 0 ? -1 : 0);
		    sign.z = (dir.z > 0) ? 1 : (dir.z < 0 ? -1 : 0);

		    reset();
	    }
	    
	    public bool next() 
	    {
		    if (plotted++ > 0) 
		    {
			    float mx = sign.x * max.x;
			    float my = sign.y * max.y;
			    float mz = sign.z * max.z;
			
			    if (mx < my && mx < mz) 
			    {
				    max.x += delta.x;
				    index.x += sign.x;
			    }
			    else if (mz < my && mz < mx) 
			    {
				    max.z += delta.z;
				    index.z += sign.z;
			    }
			    else 
			    {
				    max.y += delta.y;
				    index.y += sign.y;
			    }
		    }
		    return (plotted <= limit);
	    }
	
	    public void reset() 
	    {
		    plotted = 0;
		
		    index.x = (int)Math.Floor((pos.x - off.x) / size.x);
		    index.y = (int)Math.Floor((pos.y - off.y) / size.y);
		    index.z = (int)Math.Floor((pos.z - off.z) / size.z);
		
		    float ax = index.x * size.x + off.x;
		    float ay = index.y * size.y + off.y;
		    float az = index.z * size.z + off.z;

		    max.x = (sign.x > 0) ? ax + size.x - pos.x : pos.x - ax;
		    max.y = (sign.y > 0) ? ay + size.y - pos.y : pos.y - ay;
		    max.z = (sign.z > 0) ? az + size.z - pos.z : pos.z - az;
		    max.Div( dir );
	    }

	    
	    public void end()
	    {
		    plotted = limit + 1;
	    }
	
	    
	    public Vec3i get() 
	    {
		    return index;
	    }
	
	    public Vec3f actual(Vec3f outV) {
		    outV.x = index.x * size.x + off.x;
		    outV.y = index.y * size.y + off.y;
		    outV.z = index.z * size.z + off.z;
            return outV;
	    }
	
	    public Vec3f actual() {
		    return actual( new Vec3f() );
	    }

	    public Vec3f Size() {
		    return size;
	    }
	
	    public void Size(float w, float h, float d) {
		    size.Set(w, h, d);
	    }
	
	    public Vec3f offset() {
		    return off;
	    }
	
	    public void offset(float x, float y, float z) {
		    off.Set(x, y, z);
	    }
	
	    public Vec3f Position() {
		    return pos;
	    }
	
	    public Vec3f Direction() {
		    return dir;
	    }
	
	    public Vec3i Sign() {
		    return sign;
	    }
	
	    public Vec3f Delta() {
		    return delta;
	    }
	
	    public Vec3f Max() {
		    return max;
	    }
	
	    public int Limit() {
		    return limit;
	    }
	
	    public int Plotted() {
		    return plotted;
	    }
    }
}
