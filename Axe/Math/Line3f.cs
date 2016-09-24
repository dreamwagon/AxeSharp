using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class Line3f 
    {

	    public Vec3f s;
	    public Vec3f e;

        public Line3f()
            : this(new Vec3f(), new Vec3f()) { }

        public Line3f(float x0, float y0, float z0, float x1, float y1, float z1)
            : this(new Vec3f(x0, y0, z0), new Vec3f(x1, y1, z1)) { }

        public Line3f(Line3f line) 
            : this(line.s, line.e) { }
	
	    public Line3f(Vec3f s, Vec3f e) {
		    this.s = s;
		    this.e = e;
	    }

        public void Set(Line3f line)
        {
            s.Set(line.s);
            e.Set(line.e);
	    }

        public void Set(float x0, float y0, float z0, float x1, float y1, float z1)
        {
            s.Set(x0, y0, z0);
            e.Set(x1, y1, z1);
	    }

        public void Set(Vec3f start, Vec3f end)
        {
            s.Set(start);
            e.Set(end);
	    }
	
	    public float length() {
		    return s.Distance(e);
	    }
	
	    public Vec3f dir() {
		    Vec3f v = new Vec3f();
		    v.norm(e.x - s.x, e.y - s.y, e.z - s.z);
		    return v;
	    }
	
	    public Vec3f diff() {
		    return new Vec3f(e.x - s.x, e.y - s.y, e.z - s.z);
	    }
	
	    public String toString() {
		    return String.Format("{(%.2f, %.2f, %.2f) => (%.2f, %.2f ,%.2f)}", s.x, s.y, s.z, e.x, e.y, e.z);
	    }
	
    }
}
