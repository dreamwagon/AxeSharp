using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
    public class Bound3i : Attribute<Bound3i>
    {
	    public int l, r, t, b, n, f;

	    public Bound3i() {
	    }

	    public Bound3i(int x, int y, int z) {
		    clear(x, y, z);
	    }

	    public Bound3i(int left, int top, int right, int bottom, int near, int far) {
		    Set(left, top, right, bottom, near, far);
	    }

	    public Bound3i(Bound3i x) {
		    Set(x);
	    }
	
	    public Bound3i(Vec3i x, Vec3i y) {
		    clear( x );
		    include( y );
	    }

	    public void include(Vec3i v) {
		    include( v.x, v.y, v.z );
	    }
	
	    public void include(int x, int y, int z) {
		    if (x < l) l = x;
		    if (x > r) r = x;
		    if (y > t) t = y;
		    if (y < b) b = y;
		    if (z < n) n = z;
		    if (z > f) f = z;
	    }

	    public void clear(Vec3i v) {
		    clear( v.x, v.y, v.z );
	    }
	
	    public void clear(int x, int y, int z) {
		    l = r = x;
		    t = b = y;
		    n = f = z;
	    }

	    public void clear() {
		    l = r = t = b = n = f = 0;
	    }
	
	    public void translate(Vec3i v) {
		    translate( v.x, v.y, v.z );
	    }
	
	    public void translate(int dx, int dy, int dz) {
		    l += dx;
		    r += dx;
		    t += dy;
		    b += dy;
		    n += dz;
		    f += dz;
	    }

	    public void Set(Bound3i x) {
		    Set(x.l, x.t, x.r, x.b, x.n, x.f);
	    }

	    public void Set(int left, int top, int right, int bottom, int near, int far) {
		    l = left;
		    r = right;
		    t = top;
		    b = bottom;
		    n = near;
		    f = far;
	    }

	    public void rect(int x, int y, int z, int width, int height, int depth) {
		    Set(x, y, x + width, y + height, z, z + depth);
	    }

	    public void line(int x0, int y0, int z0, int x1, int y1, int z1) {
		    l = Math.Min(x0, x1);
		    r = Math.Max(x0, x1);
            t = Math.Max(y0, y1);
            b = Math.Min(y0, y1);
            n = Math.Min(z0, z1);
            f = Math.Max(z0, z1);
	    }

	    public void ellipse(int cx, int cy, int cz, int rw, int rh, int rz) {
		    rw = Math.Abs(rw);
		    rh = Math.Abs(rh);
		    rz = Math.Abs(rz);
		    l = cx - rw;
		    r = cx + rw;
		    t = cy - rh;
		    b = cy + rh;
		    n = cz - rz;
		    f = cz + rz;
	    }

	    public void center(int x, int y, int z) {
		    int w = width() >> 1;
		    int h = height() >> 1;
		    int d = depth() >> 1;
		    l = x - w;
		    r = x + w;
		    t = y - h;
		    b = y + h;
		    n = z - d;
		    f = z + d;
	    }

	    public void move(int dx, int dy, int dz) {
		    l += dx; r += dx;
		    t += dy; b += dy;
		    n += dz; f += dz;
	    }

	    public void zoom(int sx, int sy, int sz) {
		    int hw = (width() >> 1) * Math.Abs(sx);
		    int hh = (height() >> 1) * Math.Abs(sy);
		    int hd = (depth() >> 1) * Math.Abs(sz);
		    ellipse(cx(), cy(), cz(), hw, hh, hd);
	    }

	    public int width() {
		    return (r - l);
	    }

	    public int height() {
		    return (t - b);
	    }

	    public int depth() {
		    return (f - n);
	    }

	    public int area() {
		    return (r - l) * (t - b) * (f - n);
	    }

	    public int cx() {
		    return (l + r) >> 1;
	    }

	    public int cy() {
		    return (t + b) >> 1;
	    }

	    public int cz() {
		    return (n + f) >> 1;
	    }

	    public void intersection(Bound3i x, Bound3i y) {
            l = Math.Max(x.l, y.l);
		    r = Math.Min(x.r, y.r);
            t = Math.Min(x.t, y.t);
            b = Math.Max(x.b, y.b);
            n = Math.Max(x.n, y.n);
            f = Math.Min(x.f, y.f);
	    }

	    public void union(Bound3i x, Bound3i y) {
		    l = Math.Min(x.l, y.l);
		    r = Math.Max(x.r, y.r);
            t = Math.Max(x.t, y.t);
            b = Math.Min(x.b, y.b);
            n = Math.Min(x.n, y.n);
            f = Math.Max(x.f, y.f);
	    }

	    public bool intersects(Bound3i x) {
		    return !(x.l >= r || x.r <= l || x.t <= b || x.b >= t || x.n >= f || x.f <= n);
	    }

	    public bool touches(Bound3i x) {
		    return !(x.l > r || x.r < l || x.t < b || x.b > t || x.n > f || x.f < n);
	    }

	    public bool contains(Bound3i x) {
		    return !(x.l <= l || x.r >= r || x.t >= t || x.b <= b || x.n <= n || x.f >= f);
	    }

	    public bool inside(Bound3i x) {
		    return !(x.l < l || x.r > r || x.t > t || x.b < b || x.n < n || x.f > f);
	    }

	    public String toString() {
		    return String.Format("{x=%d, y=%d, z=%d, w=%d, h=%d, d=%d}", l, b, n, width(), height(), depth());
	    }

	    public bool contains(int x, int y, int z) {
		    return !(x < l || x > r || y > t || y < b || z > f || z < n);
	    }

	    
	    public Bound3i Get()
	    {
		    return this;
	    }

	    public void Interpolate(Bound3i start, Bound3i end, float delta)
	    {
		    l = (int)((end.l - start.l) * delta + start.l);
		    t = (int)((end.t - start.t) * delta + start.t);
		    r = (int)((end.r - start.r) * delta + start.r);
		    b = (int)((end.b - start.b) * delta + start.b);
		    n = (int)((end.n - start.n) * delta + start.n);
		    f = (int)((end.f - start.f) * delta + start.f);
	    }
	
	    
	    public void Add( Bound3i value, float delta )
	    {
		    l += (int)(value.l * delta);
            t += (int)(value.t * delta);
            r += (int)(value.r * delta);
            b += (int)(value.b * delta);
            n += (int)(value.n * delta);
            f += (int)(value.f * delta);
	    }


	    
	    public float Distance(Bound3i to)
	    {
		    return Math.Abs(to.area() - area());
	    }

	    // Animation

	    /*public Event<Bound3i> animateTo(Bound3i end, float delay, float duration)
	    {
		    return animateTo(end, delay, duration, Easings.Linear);
	    }

	    public Event<Bound3i> animateTo(Bound3i end, float delay, float duration, EasingMethod method)
	    {
		    return new Event<Bound3i>(this,
				    new Tween<Bound3i>(new Bound3i(this), new Bound3i(end)),
				    delay, duration, 0, 1, Easings.In, method);
	    }*/

	    // Remote
	
	    public static int SIZE = 24;
	
	    
	    public bool IsEqual( Bound3i value ) {
		    return ( l == value.l && t == value.t && r == value.r && b == value.b && n == value.n && f == value.f );
	    }
	    
	    public void Copy( Bound3i from, Bound3i to ) {
		    to.l = from.l;
		    to.t = from.t;
		    to.r = from.r;
		    to.b = from.b;
		    to.n = from.n;
		    to.f = from.f;
	    }
	
	    
	    public Bound3i Clone() {
		    return new Bound3i( l, t, r, b, n, f );
	    }
	
	    
	    public void Update( Bound3i value ) {
		    value.l = l;
		    value.t = t;
		    value.r = r;
		    value.b = b;
		    value.n = n;
		    value.f = f;
	    }	
	
	    
	    public void Mul( Bound3i value ) {
		    l *= value.l;
		    t *= value.t;
		    r *= value.r;
		    b *= value.b;
		    n *= value.n;
		    f *= value.f;
	    }

	    
	    public void Scale( float d ) {
		    l *= (int)d;
            t *= (int)d;
            r *= (int)d;
            b *= (int)d;
            n *= (int)d;
            f *= (int)d;
	    }

	    
	    public Attribute<Bound3i> Create() {
		    return new Bound3i();
	    }

	    
	    public void write( MemoryStream outMem ) 
        {
            using (BinaryWriter bw = new BinaryWriter(outMem))
            {
                bw.Write(l);
                bw.Write(t);
                bw.Write(r);
                bw.Write(b);
                bw.Write(n);
                bw.Write(f);
            }
	    }


        public void read(MemoryStream inMem)
        {
            using (BinaryReader br = new BinaryReader(inMem))
            {
                l = br.ReadInt16();
                t = br.ReadInt16();
                r = br.ReadInt16();
                b = br.ReadInt16();
                n = br.ReadInt16();
                f = br.ReadInt16();
            }
	    }

	    
	    public int bytes() {
		    return SIZE;
	    }

	    
	    public void read( InputModel input )
	    {
		    t = input.readInt( "t" );
		    b = input.readInt( "b" );
		    r = input.readInt( "r" );
		    l = input.readInt( "l" );
		    n = input.readInt( "n" );
		    f = input.readInt( "f" );
	    }

	    
	    public void write( OutputModel output )
	    {
		    output.write( "t", t );
		    output.write( "b", b );
		    output.write( "r", r );
		    output.write( "l", l );
		    output.write( "n", n );
		    output.write( "f", f );
	    }

	    public static Bound3i getIntersection(Bound3i x, Bound3i y)
	    {
		    Bound3i b = new Bound3i();
		    b.intersection( x, y );
		    return b;
	    }

	    public static Bound3i getUnion(Bound3i x, Bound3i y)
	    {
		    Bound3i b = new Bound3i();
		    b.union( x, y );
		    return b;
	    }
	
    }
}
