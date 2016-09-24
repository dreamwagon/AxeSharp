using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    public class Vec3i : Attribute<Vec3i>
    {	
	    public static Vec3i RIGHT = new Vec3i(1, 0, 0);
	    public static Vec3i LEFT = new Vec3i(-1, 0, 0);
	    public static Vec3i UP = new Vec3i(0, 1, 0);
	    public static Vec3i DOWN = new Vec3i(0, -1, 0);
	    public static Vec3i NEAR = new Vec3i(0, 0, 1);
	    public static Vec3i FAR = new Vec3i(0, 0, -1);
	    public static Vec3i ZERO = new Vec3i(0, 0, 0);
	    public static Vec3i ONE = new Vec3i(1, 1, 1);
	
	    public int x, y, z;
	    public Vec3i() {
	    }
	    public Vec3i(Vec3i v) {
		    Set(v);
	    }
	    public Vec3i(int v) {
		    x = y = z = v;
	    }
	    public Vec3i(int x, int y, int z) {
		    Set(x, y, z);
	    }
	    public Vec3i( Vec3f v) {
            Set((int)v.x, (int)v.y, (int)v.z);
	    }
        public void Set(Vec3i v)
        {
		    x = v.x;
		    y = v.y;
		    z = v.z;
	    }
        public void Set(int x, int y, int z)
        {
		    this.x = x;
		    this.y = y;
		    this.z = z;
	    }
	    public void cross(Vec3i a, Vec3i b) {
		    x = a.y * b.z - b.y * a.z;
		    y = a.z * b.x - b.z * a.x;
		    z = a.x * b.y - b.x * a.y;
	    }
	    public void norm() {
		    float sq = (x*x)+ (y*y) + (z*z);
		    if (sq != 0.0f && sq != 1.0f) {
			    div((float)Math.Sqrt(sq));
		    }
	    }
	    public void norm(Vec3i v) {
            Set(v); norm();
	    }
	    public void norm(int x, int y, int z) {
            Set(x, y, z); norm();
	    }
	    public void cross(Vec3i r, Vec3i o, Vec3i l) {
		    float x0 = r.x-o.x, y0 = r.y-o.y, z0 = r.z-o.z;
		    float x1 = l.x-o.x, y1 = l.y-o.y, z1 = l.z-o.z;
		    float d0 = (float)(1.0 / Math.Sqrt((x0*x0)+(y0*y0)+(z0*z0)));
		    float d1 = (float)(1.0 / Math.Sqrt((x1*x1)+(y1*y1)+(z1*z1)));
		    x0 *= d0; y0 *= d0; z0 *= d0;
		    x1 *= d1; y1 *= d1; z0 *= d1;
		    x = (int)(y0 * z1 - y1 * z0);
		    y = (int)(z0 * x1 - z1 * x0);
		    z = (int)(x0 * y1 - x1 * y0);
	    }
	    public void reflect(Vec3i v, Vec3i n) {
            Set(v); Add(n, -2f * v.dot(n));
	    }
	    public void refract(Vec3i v, Vec3i n) {
		    reflect(v, n); neg();
	    }
	    public void div(float s) {
		    if (s != 0.0) {
			    s = 1f / s;
                x *= (int)s;
                y *= (int)s;
                z *= (int)s;
		    }
	    }
	    public void Mul(float s) {
		    x *= (int)s;
            y *= (int)s;
            z *= (int)s;
	    }
        public void Add(Vec3i v)
        {
		    x += v.x; y += v.y; z += v.z;
	    }
        public void Add(float vx, float vy, float vz)
        {
		    x += (int)vx; 
		    y += (int)vy; 
		    z += (int)vz;
	    }
        public void Add(Vec3i v, float scale)
        {
		    x += (int)(v.x * scale); 
		    y += (int)(v.y * scale);
		    z += (int)(v.z * scale);
	    }
	    public void Sub(Vec3i v) {
		    x -= v.x; 
		    y -= v.y; 
		    z -= v.z;
	    }
	    public void neg() {
		    x = -x; 
		    y = -y; 
		    z = -z;
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
	    public float dot(Vec3i v) {
		    return (x * v.x) + (y * v.y) + (z * v.z);
	    }
	    public float length() {
		    return (float)Math.Sqrt(dot());
	    }
	    public float Distance(Vec3i v) {
		    float dx = v.x - x;
		    float dy = v.y - y;
		    float dz = v.z - z;
		    return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
	    }
	    public bool isZero() {
		    return (x == 0f && y == 0f && z == 0f);
	    }
	    public void average(Vec3i[] v) {
		    if (v.Length > 0) {
			    x = y = z = 0;
                for (int i = 0; i < v.Length; i++)
                {
				    Add(v[i]);
			    }
                div(v.Length);
		    }
	    }

	    /*public void glTranslate() {
		    glTranslatef(x, y, z);
	    }
	    public void glRotate(float degrees) {
		    glRotatef(degrees, x, y, z);
	    }
	    public void glScale() {
		    glScalef(x, y, z);
	    }
	    public void bind() {
		    glVertex3f(x, y, z);
	    }*/

	    public void Interpolate(Vec3i start, Vec3i end, float delta) {
		    x = (int)((end.x - start.x) * delta) + start.x;
		    y = (int)((end.y - start.y) * delta) + start.y;
		    z = (int)((end.z - start.z) * delta) + start.z;
	    }
	    public Vec3i Get() {
		    return this;
	    }
	    public void max(int max) {
		    float lengthSq = dot();
		    if (lengthSq > max * max) {
			    int length = (int)Math.Sqrt(lengthSq);
			    Mul(max / length);
		    }
	    }
	    public bool equals(int vx, int vy, int vz) {
		    return (x == vx && y == vy && z == vz);
	    }
	
	    public void max(Vec3i max) {
		    if (x > max.x) {
			    x = max.x;
		    }
		    if (y > max.y) {
			    y = max.y;
		    }
		    if (z > max.z) {
			    z = max.z;
		    }
	    }
	
	    public void min(Vec3i min) {
		    if (x < min.x) {
			    x = min.x;
		    }
		    if (y < min.y) {
			    y = min.y;
		    }
		    if (z < min.z) {
			    z = min.z;
		    }
	    }

	    public int hashCode() {
		    return (x << 16) + (y << 8) + z;
	    }

	    public override bool Equals(Object o)
        {
		    if (o is Vec3i) {
			    return IsEqual((Vec3i)o);
		    }
		    return false;
	    }

	    public static Vec3i Mul(Vec3i v, float s) {
		    return new Vec3i((int)(v.x * s), (int)(v.y * s), (int)(v.z * s));
	    }
	    public static Vec3i Mul(Vec3i v, Vec3i w) {
		    return new Vec3i(v.x * w.x, v.y * w.y, v.z * w.z);
	    }
	    public static Vec3i inter(Vec3i s, Vec3i e, float delta) {
		    return new Vec3i((int)((e.x - s.x) * delta) + s.x,
				    (int)((e.y - s.y) * delta) + s.y, 
				    (int)((e.z - s.z) * delta) + s.z);
	    }
	    public static Vec3i Sub(Vec3i a, Vec3i b) { 
		    return new Vec3i(a.x - b.x, a.y - b.y, a.z - b.z);
	    }
	    public static Vec3i Add(Vec3i a, Vec3i b) { 
		    return new Vec3i(a.x + b.x, a.y + b.y, a.z + b.z);
	    }
	    public float distance(Vec3i start, Vec3i end) {
		    return start.Distance(end);
	    }
	
	    public String toString() {
		    return String.Format("{%d, %d, %d}", x, y, z);
	    }

	    // Animation

	    /*public Event<Vec3i> animateTo(Vec3i end, float delay, float duration) 
	    {
		    return animateTo(end, delay, duration, Easings.Linear);
	    }

	    public Event<Vec3i> animateTo(Vec3i end, float delay, float duration, EasingMethod method) 
	    {
		    return new Event<Vec3i>(this, 
				    new Tween<Vec3i>(new Vec3i(this), new Vec3i(end)), 
				    delay, duration, 0, 1, Easings.In, method);
	    }*/
	
	    // Remote
	
	    public static int SIZE = 12;
	
	    public bool isEqual( int vx, int vy, int vz ) {
		    return ( x == vx && y == vy && z == vz );
	    }
	
	
	    public bool IsEqual( Vec3i v ) {
		    return ( x == v.x && y == v.y && z == v.z );
	    }

	
	    public void Copy( Vec3i from, Vec3i to ) {
		    to.x = from.x;
		    to.y = from.y;
		    to.z = from.z;
	    }
	
	
	    public Vec3i Clone() {
		    return new Vec3i( x, y, z );
	    }
	
	
	    public void Update( Vec3i value ) {
		    value.x = x;
		    value.y = y;
		    value.z = z;
	    }

	
	    public void Scale( float d ) {
		    x *= (int)d;
		    y *= (int)d;
		    z *= (int)d;
	    }

	
	    public void Mul( Vec3i value ) {
		    x *= value.x;
		    y *= value.y;
		    z *= value.z;
	    }

	
	    public Attribute<Vec3i> Create() {
		    return new Vec3i();
	    }

        Vector3 v3 = new Vector3();
        public Vector3 ToVector3()
        {
            v3.X = x;
            v3.Y = y;
            v3.Z = z;
            return v3;
        }
	
	    public void write( MemoryStream outMem ) 
        {
            using (BinaryWriter reader = new BinaryWriter(outMem))
            {
                reader.Write(x);
                reader.Write(y);
                reader.Write(z);
            }
	    }


        public void read(MemoryStream inMem )
        {
            using( BinaryReader reader = new BinaryReader( inMem ))
            {
		        x = reader.ReadInt32();
                y = reader.ReadInt32();
                z = reader.ReadInt32();
            }
	    }
	
	
	    public int bytes() {
		    return SIZE;
	    }
	
	
	    public void read( InputModel input )
	    {
		    x = input.readInt( "x" );
		    y = input.readInt( "y" );
		    z = input.readInt( "z" );
	    }
	
	
	    public void write( OutputModel output )
	    {
		    output.write( "x", x );
		    output.write( "y", y );
		    output.write( "z", z );
	    }

    }
}
