using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
    public class Bound3f : Attribute<Bound3f>
    {
	    public float l, r, t, b, n, f;

	    public Bound3f() {
	    }

	    public Bound3f(float x, float y, float z) {
		    clear(x, y, z);
	    }

	    public Bound3f(float left, float top, float right, float bottom, float near, float far) {
		    Set(left, top, right, bottom, near, far);
	    }

	    public Bound3f(Bound3f x) {
		    Set(x);
	    }

	    public void include(float x, float y, float z) {
		    if (x < l) l = x;
		    if (x > r) r = x;
		    if (y > t) t = y;
		    if (y < b) b = y;
		    if (z < n) n = z;
		    if (z > f) f = z;
	    }
	
	    public void include(Vec3f v) {
		    include( v.x, v.y, v.z );
	    }

	    public void clear(float x, float y, float z) {
		    l = r = x;
		    t = b = y;
		    n = f = z;
	    }
	
	    public void clear(Vec3f v) {
		    clear( v.x, v.y, v.z );
	    }

	    public void clear() {
		    l = r = t = b = n = f = 0;
	    }

	    public void Set(Bound3f x) {
            Set(x.l, x.t, x.r, x.b, x.n, x.f);
	    }

        public void Set(float left, float top, float right, float bottom, float near, float far)
        {
		    l = left;
		    r = right;
		    t = top;
		    b = bottom;
		    n = near;
		    f = far;
	    }

	    public void rect(float x, float y, float z, float width, float height, float depth) {
            Set(x, y, x + width, y + height, z, z + depth);
	    }

	    public void line(float x0, float y0, float z0, float x1, float y1, float z1) {
		    l = Math.Min(x0, x1);
		    r = Math.Max(x0, x1);
		    t = Math.Max(y0, y1);
		    b = Math.Min(y0, y1);
		    n = Math.Min(z0, z1);
		    f = Math.Max(z0, z1);
	    }

	    public void ellipse(float cx, float cy, float cz, float rw, float rh, float rz) {
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

	    public void center(float x, float y, float z) {
		    float w = width() * 0.5f;
		    float h = height() * 0.5f;
		    float d = depth() * 0.5f;
		    l = x - w;
		    r = x + w;
		    t = y - h;
		    b = y + h;
		    n = z - d;
		    f = z + d;
	    }

	    public void move(Vec3f delta) {
		    move( delta.x, delta.y, delta.z );
	    }
	
	    public void move(float dx, float dy, float dz) {
		    l += dx; r += dx;
		    t += dy; b += dy;
		    n += dz; f += dz;
	    }
	
	    public void expand(float gx, float gy, float gz) {
		    l -= gx;
		    r += gx;
		    t += gy;
		    b -= gy;
		    n -= gz;
		    f += gz;
	    }

	    public void zoom(float sx, float sy, float sz) {
		    float hw = width() * 0.5f * Math.Abs(sx);
		    float hh = height() * 0.5f * Math.Abs(sy);
		    float hd = depth() * 0.5f * Math.Abs(sz);
		    ellipse(cx(), cy(), cz(), hw, hh, hd);
	    }

	    public float width() {
		    return (r - l);
	    }

	    public float height() {
		    return (t - b);
	    }

	    public float depth() {
		    return (f - n);
	    }

	    public float area() {
		    return (r - l) * (t - b) * (f - n);
	    }

	    public float cx() {
		    return (l + r) * 0.5f;
	    }

	    public float cy() {
		    return (t + b) * 0.5f;
	    }

	    public float cz() {
		    return (n + f) * 0.5f;
	    }
	
	    public void getCorners(Vec3f[] x) {
		    x[0].Set(l, t, n);
            x[1].Set(l, b, n);
            x[2].Set(l, t, f);
            x[3].Set(l, b, f);
            x[4].Set(r, t, n);
            x[5].Set(r, b, n);
            x[6].Set(r, t, f);
            x[7].Set(r, b, f);
	    }

	    public void getBorder(Line3f[] x) {
		    // n => f
            x[0].Set(l, t, n, l, t, f);
            x[1].Set(l, b, n, l, b, f);
            x[2].Set(r, t, n, r, t, f);
            x[3].Set(r, b, n, r, b, f);
		    // l => r
            x[4].Set(l, t, n, r, t, n);
            x[5].Set(l, b, n, r, b, n);
            x[6].Set(l, t, f, r, t, f);
            x[7].Set(l, b, f, r, b, f);
		    // b => t
		    x[ 8].Set(l, b, n, l, t, n);
            x[9].Set(r, b, n, r, t, n);
            x[10].Set(l, b, f, l, t, f);
            x[11].Set(r, b, f, r, t, f);
	    }
	
	    public void intersection(Bound3f x, Bound3f y) {
		    l = Math.Max(x.l, y.l);
		    r = Math.Min(x.r, y.r);
		    t = Math.Min(x.t, y.t);
		    b = Math.Max(x.b, y.b);
		    n = Math.Max(x.n, y.n);
		    f = Math.Min(x.f, y.f);
	    }

	    public void union(Bound3f x, Bound3f y) {
		    l = Math.Min(x.l, y.l);
		    r = Math.Max(x.r, y.r);
		    t = Math.Max(x.t, y.t);
		    b = Math.Min(x.b, y.b);
		    n = Math.Min(x.n, y.n);
		    f = Math.Max(x.f, y.f);
	    }

	    public bool intersects(Bound3f x) {
		    return !(x.l >= r || x.r <= l || x.t <= b || x.b >= t || x.n >= f || x.f <= n);
	    }

	    public bool touches(Bound3f x) {
		    return !(x.l > r || x.r < l || x.t < b || x.b > t || x.n > f || x.f < n);
	    }

	    public bool contains(Bound3f x) {
		    return !(x.l < l || x.r > r || x.t > t || x.b < b || x.n < n || x.f > f);
	    }

	    public bool inside(Bound3f x) {
		    return !(x.l <= l || x.r >= r || x.t >= t || x.b <= b || x.n <= n || x.f >= f);
	    }

	    public String toString() {
		    return String.Format("{%.2f, %.2f, %.2f, %.2f, %.2f, %.2f}", l, t, n, width(), height(), depth());
	    }

	    public bool contains(float x, float y, float z) {
		    return !(x < l || x > r || y > t || y < b || z > f || z < n);
	    }

	    public bool contains(Vec3f v) {
		    return contains(v.x, v.y, v.z);
	    }

	    
	    public Bound3f Get()
	    {
		    return this;
	    }


	    
	    public void Interpolate(Bound3f start, Bound3f end, float delta)
	    {
		    l = (end.l - start.l) * delta + start.l;
		    t = (end.t - start.t) * delta + start.t;
		    r = (end.r - start.r) * delta + start.r;
		    b = (end.b - start.b) * delta + start.b;
		    n = (end.n - start.n) * delta + start.n;
		    f = (end.f - start.f) * delta + start.f;
	    }

	    
	    public void Add( Bound3f value, float delta )
	    {
		    l += value.l * delta;
		    t += value.t * delta;
		    r += value.r * delta;
		    b += value.b * delta;
		    n += value.n * delta;
		    f += value.f * delta;
	    }

	    
	    public float Distance(Bound3f to)
	    {
		    return Math.Abs(to.area() - area());
	    }

	    public float distanceSq( Vec3f v )
	    {
		    return 0f;
	    }
	
	    // Animation

	   /* public Event<Bound3f> animateTo(Bound3f end, float delay, float duration)
	    {
		    return animateTo(end, delay, duration, Easings.Linear);
	    }

	    public Event<Bound3f> animateTo(Bound3f end, float delay, float duration, EasingMethod method)
	    {
		    return new Event<Bound3f>(this,
				    new Tween<Bound3f>(new Bound3f(this), new Bound3f(end)),
				    delay, duration, 0, 1, Easings.In, method);
	    }*/

	    // Remote
	
	    public static int SIZE = 24;
	
	    public bool isEmpty() {
		    return ( width() == 0 && height() == 0 && depth() == 0 );
	    }
	
	    
	    public bool IsEqual( Bound3f value ) {
		    return ( l == value.l && t == value.t && r == value.r && b == value.b && n == value.n && f == value.f );
	    }
	
	    
	    public void Copy( Bound3f from, Bound3f to ) {
		    to.l = from.l;
		    to.t = from.t;
		    to.r = from.r;
		    to.b = from.b;
		    to.n = from.n;
		    to.f = from.f;
	    }
	
	    
	    public Bound3f Clone() {
		    return new Bound3f( l, t, r, b, n, f );
	    }
	
	    
	    public void Update( Bound3f value ) {
		    value.l = l;
		    value.t = t;
		    value.r = r;
		    value.b = b;
		    value.n = n;
		    value.f = f;
	    }

	    
	    public void Mul( Bound3f value ) {
		    l *= value.l;
		    t *= value.t;
		    r *= value.r;
		    b *= value.b;
		    n *= value.n;
		    f *= value.f;
	    }

	    
	    public void Scale( float d ) {
		    l *= d;
		    t *= d;
		    r *= d;
		    b *= d;
		    n *= d;
		    f *= d;
	    }

	    public Attribute<Bound3f> Create() {
		    return new Bound3f();
	    }
   
	    public void write( MemoryStream outMem ) 
        {
            using (BinaryWriter writer = new BinaryWriter(outMem))
            {
                writer.Write(l);
                writer.Write(t);
                writer.Write(r);
                writer.Write(b);
                writer.Write(n);
                writer.Write(f);
            }
	    }


        public void read(MemoryStream inMem)
        {
            using (BinaryReader reader = new BinaryReader(inMem))
            {
                l = reader.ReadInt64();
                t = reader.ReadInt64();
                r = reader.ReadInt64();
                b = reader.ReadInt64();
                n = reader.ReadInt64();
                f = reader.ReadInt64();
            }
	    }

	    
	    public int bytes() {
		    return SIZE;
	    }

	    
	    public void read( InputModel input )
	    {
		    t = input.readFloat( "t" );
		    b = input.readFloat( "b" );
		    r = input.readFloat( "r" );
		    l = input.readFloat( "l" );
		    n = input.readFloat( "n" );
		    f = input.readFloat( "f" );
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

    }
}
