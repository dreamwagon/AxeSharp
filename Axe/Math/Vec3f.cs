using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    public class Vec3f : Attribute<Vec3f>, Shape<Vec3f>
    {
	    /*public static const Factory<Vec3f> FACTORY = new Factory<Vec3f>() 
        {
		    public Vec3f create() {
			    return new Vec3f();
		    }
	    };*/

	    public static Vec3f RIGHT = new Vec3f(1, 0, 0);
	    public static Vec3f LEFT = new Vec3f(-1, 0, 0);
	    public static Vec3f UP = new Vec3f(0, 1, 0);
	    public static Vec3f DOWN = new Vec3f(0, -1, 0);
	    public static Vec3f NEAR = new Vec3f(0, 0, -1);
	    public static Vec3f FAR = new Vec3f(0, 0, 1);
	    public static Vec3f ONE = new Vec3f(1, 1, 1);
	    public static Vec3f ZERO = new Vec3f();

	    public float x, y, z;

	    public Vec3f() {
	    }
	    public Vec3f(Vec3f v) {
		    Set(v);
	    }
	    public Vec3f(float v) {
		    x = y = z = v;
	    }
	    public Vec3f(float x, float y) {
		    Set(x, y, 0);
	    }
	    public Vec3f(float x, float y, float z) {
		    Set(x, y, z);
	    }

        Vector3 v3 = new Vector3();
        public Vector3 ToVector3()
        {
            v3.X = x;
            v3.Y = y;
            v3.Z = z;
            return v3;
        }
	    public void Set(Vec3f v) {
		    x = v.x;
		    y = v.y;
		    z = v.z;
	    }
	    public void Set(float x, float y, float z) {
		    this.x = x;
		    this.y = y;
		    this.z = z;
	    }
	    public void Set(float v) {
		    x = y = z = v;
	    }
	    public void cross(Vec3f a, Vec3f b) {
		    x = a.y * b.z - b.y * a.z;
		    y = a.z * b.x - b.z * a.x;
		    z = a.x * b.y - b.x * a.y;
	    }
	    public float norm() {
		    float sq = (x*x)+ (y*y) + (z*z);
		    if (sq != 0.0f && sq != 1.0f) {
			    sq = (float)Math.Sqrt(sq);
			    Div(sq);
		    }
		    return sq;
	    }
	    public void norm(Vec3f v) {
		    Set(v); norm();
	    }
	    public void norm(float x, float y, float z) {
            Set(x, y, z); norm();
	    }
	    public void norm(Vec3f a, Vec3f b, Vec3f c) 
	    {
		    x = a.y * (b.z - c.z) + b.y * (c.z - a.z) + c.y * (a.z - b.z); 
		    y = a.z * (b.x - c.x) + b.z * (c.x - a.x) + c.z * (a.x - b.x);
		    z = a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y);
		
		    float sq = ( x * x + y * y + z * z );
		
		    if (sq != 0 && sq != 1)
		    {
			    float length = 1f / (float)Math.Sqrt( sq );
			    x *= length;
			    y *= length;
			    z *= length;
		    }
	    }
	    public void cross(Vec3f r, Vec3f o, Vec3f l) {
		    float x0 = r.x-o.x, y0 = r.y-o.y, z0 = r.z-o.z;
		    float x1 = l.x-o.x, y1 = l.y-o.y, z1 = l.z-o.z;
		    float d0 = (float)(1.0 / Math.Sqrt((x0*x0)+(y0*y0)+(z0*z0)));
		    float d1 = (float)(1.0 / Math.Sqrt((x1*x1)+(y1*y1)+(z1*z1)));
		    x0 *= d0; y0 *= d0; z0 *= d0;
		    x1 *= d1; y1 *= d1; z0 *= d1;
		    x = y0 * z1 - y1 * z0;
		    y = z0 * x1 - z1 * x0;
		    z = x0 * y1 - x1 * y0;
	    }
	    public void reflect(Vec3f v, Vec3f n) {
            Set(v); Add(n, -2f * v.dot(n));
	    }
	    public void refract(Vec3f v, Vec3f n) {
		    reflect(v, n); neg();
	    }
	    public void Div(float s) {
		    if (s != 0.0) {
			    s = 1f / s;
			    x *= s;
			    y *= s;
			    z *= s;
		    }
	    }
	    public void Mul(float s) {
		    x *= s;
		    y *= s;
		    z *= s;
	    }
	    public void Mul(Vec3f v) {
		    x *= v.x;
		    y *= v.y;
		    z *= v.z;
	    }
	    public void Div(Vec3f v) {
		    if (v.x != 0) {
			    x /= v.x;
		    }
		    if (v.y != 0) {
			    y /= v.y;
		    }
		    if (v.z != 0) {
			    z /= v.z;
		    }
	    }
	    public void Add(Vec3f v) {
		    x += v.x; y += v.y; z += v.z;
	    }
	    public void Add(float vx, float vy, float vz) {
		    x += vx; y += vy; z += vz;
	    }
	    public void Add(Vec3f v, float scale) {
		    x += v.x * scale; 
		    y += v.y * scale; 
		    z += v.z * scale;
	    }
	    public void Sub(Vec3f v) {
		    x -= v.x; y -= v.y; z -= v.z;
	    }
	    public void neg() {
		    x = -x; y = -y; z = -z;
	    }
	    public void length(float length) {
		    float sq = (x*x) + (y*y) + (z*z);
		    if (sq != 0.0 && sq != length * length) {
			    Mul(length / (float)Math.Sqrt(sq));
		    }
	    }
	    public float dot() {
		    return (x * x) + (y * y) + (z * z);
	    }
	    public float dot(Vec3f v) {
		    return (x * v.x) + (y * v.y) + (z * v.z);
	    }
	    public float length() {
		    return (float)Math.Sqrt(dot());
	    }
	    public float distanceSq(Vec3f v) {
		    float dx = v.x - x;
		    float dy = v.y - y;
		    float dz = v.z - z;
		    return (dx * dx + dy * dy + dz * dz);
	    }
	    public float Distance(Vec3f v) {
		    float dx = v.x - x;
		    float dy = v.y - y;
		    float dz = v.z - z;
		    return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
	    }
	    public bool isZero() {
		    return (x == 0f && y == 0f && z == 0f);
	    }
	    public void average(Vec3f[] v) {
		    if (v.Length > 0) {
			    x = y = z = 0f;
			    for (int i = 0; i < v.Length; i++) {
				    Add(v[i]);
			    }
			    Div(v.Length);
		    }
	    }
	    public void bind() {
		   // glVertex3f(x, y, z);
	    }
	    public void Interpolate(Vec3f start, Vec3f end, float delta) {
		    x = (end.x - start.x) * delta + start.x;
		    y = (end.y - start.y) * delta + start.y;
		    z = (end.z - start.z) * delta + start.z;
	    }
	    public Vec3f Get() {
		    return this;
	    }
	    public void max(Vec3f max) {
		    float lengthSq = dot();
		    float longestSq = max.dot();
		    if (lengthSq > longestSq) {
			    float length = (float)Math.Sqrt(lengthSq);
			    float longest = (float)Math.Sqrt(longestSq);
			    Mul(longest / length);
		    }
	    }
	
	    public float yaw() {
		    return (float)Math.Atan2( x, z );
	    }
	    public float pitch() {
		    return (float)Math.Atan2( y, (float)Math.Sqrt(x * x + z * z) );
	    }
	
	    public float angle(Vec3f v) 
	    {
		    float a = length();
		    float b = v.length();
		
		    if ( a == 0 || b == 0 ) {
			    return 0;
		    }
		
		    return (float)Math.Acos( dot( v ) / (a * b) );
	    }
	
	    public float angleNormalized(Vec3f v) 
	    {
		    return (float)Math.Acos( dot(v) );
	    }

	    public override bool Equals(Object o) 
        {
            Vec3f v = (Vec3f)o;
		    return (x == v.x && y == v.y && z == v.y);
	    }

	    public bool equals(float vx, float vy, float vz) {
		    return (x == vx && y == vy && z == vz);
	    }
	    public int hashCode() {
		    //return Float.floatToRawIntBits( x ) + Float.floatToRawIntBits( y ) + Float.floatToRawIntBits( z );
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
	    }

	    public static Vec3f Mul(Vec3f v, float s) {
		    return new Vec3f(v.x * s, v.y * s, v.z * s);
	    }
	    public static Vec3f inter(Vec3f s, Vec3f e, float delta) {
		    return new Vec3f((e.x - s.x) * delta + s.x,
				    (e.y - s.y) * delta + s.y, (e.z - s.z) * delta + s.z);
	    }
	    public static Vec3f Sub(Vec3f a, Vec3f b) {
		    return new Vec3f(a.x - b.x, a.y - b.y, a.z - b.z);
	    }
	    public static Vec3f newAdd(Vec3f a, Vec3f b) {
		    return new Vec3f(a.x + b.x, a.y + b.y, a.z + b.z);
	    }
	    public static Vec3f newScaled(Vec3f a, float scale) {
		    return new Vec3f(a.x * scale, a.y * scale, a.z * scale);
	    }
	    public static Vec3f newCross(Vec3f a, Vec3f b) {
		    Vec3f x = new Vec3f();
		    x.cross( a, b );
		    return x;
	    }
	    public static Vec3f newUnitCross(Vec3f a, Vec3f b) {
		    Vec3f x = new Vec3f();
		    x.cross( a, b );
		    x.norm();
		    return x;
	    }
	    public static Vec3f newNormal(Vec3f origin, Vec3f point) {
		    Vec3f x = new Vec3f();
		    x.Set( point );
		    x.Sub( origin );
		    x.norm();
		    return x;
	    }
	    public static Vec3f newNormal(Vec3f v) {
		    Vec3f x = new Vec3f();
		    x.norm( v );
		    return x;
	    }
	    public static void setComplementBasis(Vec3f a, Vec3f b, Vec3f n) 
	    {
	        if (Math.Abs(n.x) >= Math.Abs(n.y))
	        {
	            // n.x or n.z is the largest magnitude component, swap them
	            float invLength = 1.0f / (float)Math.Sqrt(n.x * n.x + n.z * n.z);
	            a.x = -n.z * invLength;
	            a.y = 0.0f;
	            a.z = +n.x * invLength;
	            b.x = n.y * a.z;
	            b.y = n.z * a.x - n.x * a.z;
	            b.z = -n.y * a.x;
	        }
	        else
	        {
	            // n.y or n.z is the largest magnitude component, swap them
	            float invLength = 1.0f / (float)Math.Sqrt(n.y * n.y + n.z * n.z);
	            a.x = 0.0f;
	            a.y = +n.z * invLength;
	            a.z = -n.y * invLength;
	            b.x = n.y * a.z - n.z * a.y;
	            b.y = -n.x * a.z;
	            b.z = n.x * a.y;
	        }
	    }
	    public static float distanceSq(Vec3f a, Vec3f b) {
		    return a.distanceSq( b );
	    }
	
	    public static float Distance(Vec3f a, Vec3f b) {
		    return a.Distance(b);
	    }

	    public String toString() {
		    return String.Format("{%.2f, %.2f, %.2f}", x, y, z);
	    }
	
	    public Vec3f Clone() {
		    return new Vec3f(x, y, z);
	    }

	    // Animation

	    /*public Event<Vec3f> animateTo(Vec3f end, float delay, float duration)
	    {
		    return animateTo(end, delay, duration, Easings.Linear);
	    }
        /
	    public Event<Vec3f> animateTo(Vec3f end, float delay, float duration, EasingMethod method)
	    {
		    return new Event<Vec3f>(this,
				    new Tween<Vec3f>(new Vec3f(this), new Vec3f(end)),
				    delay, duration, 0, 1, Easings.In, method);
	    }*/
	
	    // Remote
	
	    public static int SIZE = 12;
	
	    public bool IsEqual( float vx, float vy, float vz ) {
		    return ( x == vx && y == vy && z == vz );
	    }
	
	
	    public bool IsEqual( Vec3f value ) {
		    return ( x == value.x && y == value.y && z == value.z );
	    }
	
	    public bool IsEqual( Vec3f value, float epsilon ) {
		    return Numbers.equals( x, value.x, epsilon ) &&
                   Numbers.equals(y, value.y, epsilon) &&
                   Numbers.equals(z, value.z, epsilon);
	    }

	
	    public void Copy( Vec3f from, Vec3f to ) {
		    to.x = from.x;
		    to.y = from.y;
		    to.z = from.z;
	    }
	
	
	    public void Update( Vec3f value ) {
		    value.x = x;
		    value.y = y;
		    value.z = z;
	    } 

	
	    public void Scale( float d ) {
		    x *= d;
		    y *= d;
		    z *= d;
	    }

	
	    public Attribute<Vec3f> Create() {
		    return new Vec3f();
	    }
	
	
	    public void write( MemoryStream outMem ) 
        {
            using (BinaryWriter bw = new BinaryWriter(outMem))
            {
                bw.Write(x);
                bw.Write(y);
                bw.Write(z);
            }
	    }


        public void read(MemoryStream inMem)
        {
            using (BinaryReader br = new BinaryReader(inMem))
            {
                x = br.ReadInt64();
                y = br.ReadInt64();
                z = br.ReadInt64();
            }
	    }
	
	
	    public int bytes() {
		    return SIZE;
	    }
	
	
	    public void read( InputModel input )
	    {
		    x = input.readFloat( "x" );
		    y = input.readFloat( "y" );
		    z = input.readFloat( "z" );
	    }
	
	
	    public void write( OutputModel output )
	    {
		    output.write( "x", x );
		    output.write( "y", y );
		    output.write( "z", z );
	    }
	
	    public Vec3f copy()
	    {
		    return null;
	    }
	
	    public void copyTo( Vec3f outV )
	    {
		    outV.x = x;
		    outV.y = y;
            outV.z = z;
	    }
	
	
	    public void move( float t, Vec3f velocity )
	    {
		    Add( velocity, t );
	    }

    }
}
