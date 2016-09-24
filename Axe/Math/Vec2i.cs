using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
    public class Vec2i : Attribute<Vec2i>
    {
       // public static Factory<Vec2i> FACTORY = new Factory<Vec2i>(Vec2i.createVec2f());
	
	    public static Vec2i UP = new Vec2i(0, 1);
	    public static Vec2i DOWN = new Vec2i(0, -1);
	    public static Vec2i RIGHT = new Vec2i(1, 0);
	    public static Vec2i LEFT = new Vec2i(-1, 0);
	    public static Vec2i ONE = new Vec2i(1);
	    public static Vec2i ZERO = new Vec2i(0);
	
	    public int x, y;

	    public Vec2i() {
	    }

	    public Vec2i(int x, int y) {
		    Set(x, y);
	    }

	    public Vec2i(int v) {
		    x = y = v;
	    }

	    public Vec2i(Vec2i v) {
		    Set(v);
	    }
	
	    public Vec2i(Vec2i v, float scale) {
		    Set( v, scale );
	    }

	    public void Set(Vec2i v) {
		    x = v.x;
		    y = v.y;
	    }

	    public void Set(Vec2i v, float scale) {
		    x = (int)(v.x * scale);
		    y = (int)(v.y * scale);
	    }
	    public void Set( int v ) {
		    x = y = v;
	    }
	    public void Set(int x, int y) {
		    this.x = x;
		    this.y = y;
	    }
	    public void norm() 
	    {
		    int sq = (x * x) + (y * y);
		
		    if (sq != 0 && sq != 1) 
		    {
			    div( (float)Math.Sqrt(sq) );
		    }
	    }
	    public void norm(Vec2i v) {
		    Set(v); norm();
	    }
	    public void norm(int x, int y) {
		    Set(x, y); norm();
	    }
	    public void reflect(Vec2i v, Vec2i n) {
		    Set(v); Add(n, -2f * v.dot(n));
	    }
	    public void refract(Vec2i v, Vec2i n) {
		    reflect(v, n); neg();
	    }
	    public void tangent() {
		    int t = x;
		    x = y;
		    y = t;
	    }
	    public void angle(float radians, float magnitude) {
		    x = (int)(Math.Cos( radians ) * magnitude);
		    y = (int)(Math.Sin( radians ) * magnitude);
	    }
	    public float angle() {
		    float a = (float)Math.Atan2( y, x );
		    if ( a < 0) {
			    a = Scalarf.PI2 + a;
		    }
		    return a;
	    }
	    public float angleTo(Vec2i to)
	    {
		    float a = (float)Math.Atan2( to.y - y, to.x - x );
		    if ( a < 0 ) {
			    a = Scalarf.PI2 + a;
		    }
		    return a;
	    }
	    public void div(float s) {
		    if (s != 0.0) {
			    s = 1f / s;
                x *= (int)s;
                y *= (int)s;
		    }
	    }
	    public void Mul(float s) {
		    x *= (int)s;
            y *= (int)s;
	    }
	    public void Add(Vec2i v) {
		    x += v.x; 
		    y += v.y;
	    }
	    public void Add(float vx, float vy) {
            x += (int)vx;
            y += (int)vy;
	    }
	    public void Add(Vec2i v, float scale) {
            x += (int)(v.x * scale);
            y += (int)(v.y * scale);
	    }
	    public void Sub(Vec2i v) {
		    x -= v.x; 
		    y -= v.y;
	    }
	    public void Mul(Vec2i v) {
		    x *= v.x;
		    y *= v.y;
	    }
	    public void Div(Vec2i v) {
		    if ( v.x != 0 ) {
			    x /= v.x;
		    }
		    if ( v.y != 0 ) {
			    y /= v.y;
		    }
	    }
	    public void neg() {
		    x = -x; 
		    y = -y;
	    }
	    public void length(int length) 
	    {
		    int sq = (x * x) + (y * y);
		
		    if (sq != 0 && sq != length * length) 
		    {
			    Mul(length / (float)Math.Sqrt(sq));
		    }
	    }
	    public void clamp(int min, int max) 
	    {
		    int sq = (x * x) + (y * y);
		    if ( sq != 0 )
		    {
			    int sqmin = min * min;
			    int sqmax = max * max;
			    if ( sq < sqmin ) {
				    Mul( min / (float)Math.Sqrt( sq ) );
			    }
			    else if ( sq > sqmax ) {
				    Mul( max / (float)Math.Sqrt( sq ) );
			    }
		    }
	    }
	    public void rotate( float cos, float sin ) {
		    int ox = x;
		    int oy = y;
		    x = (int)(cos * ox + sin * oy);
		    y = (int)(cos * oy - sin * ox);
	    }
	    public void rotate( float angle ) {
		    rotate( (float)Math.Cos(angle), -(float)Math.Sin(angle) );
	    }
	    public int dot() {
		    return (x * x) + (y * y);
	    }
	    public int dot(Vec2i v) {
		    return (x * v.x) + (y * v.y);
	    }
	    public int dot(int vx, int vy) {
		    return (x * vx) + (y * vy);
	    }
	    public int cross(Vec2i v) {
		    return (y * v.x) - (x * v.y);
	    }
	    public int cross(int vx, int vy) {
		    return (y * vx) - (x * vy);
	    }
	    public float length() {
		    return (float)Math.Sqrt(dot());
	    }
	    public float distanceSq(Vec2i v) {
		    int dx = v.x - x;
		    int dy = v.y - y;
		    return ( dx * dx + dy * dy );
	    }
	    public float Distance(Vec2i v) {
		    int dx = v.x - x;
		    int dy = v.y - y;
		    return (float)Math.Sqrt(dx * dx + dy * dy);
	    }
	    public void clamp( int minX, int maxX, int minY, int maxY ) {
		    if ( x < minX ) x = minX;
		    if ( x > maxX ) x = maxX;
		    if ( y < minY ) y = minY;
		    if ( y > maxY ) y = maxY;
	    }
	    public bool isZero() {
		    return (x == 0 && y == 0);
	    }
	    public void average(Vec2i[] v) {
		    if (v.Length > 0) {
			    x = y = 0;
			    for (int i = 0; i < v.Length; i++) {
				    Add(v[i]);
			    }
                div(v.Length);
		    }
	    }
	   
        /*public void glTranslate() {
		    glTranslatef(x, y, 0);
	    }
	    public void glRotate(float degrees) {
		    glRotatef(degrees, x, y, 0);
	    }
	    public void glScale() {
		    glScalef(x, y, 0);
	    }
	    public void bind() {
		    glVertex3f(x, y, 0);
	    }*/

	    public void Interpolate(Vec2i start, Vec2i end, float delta) {
		    x = (int)((end.x - start.x) * delta) + start.x;
		    y = (int)((end.y - start.y) * delta) + start.y;
	    }
	    public Vec2i Get() {
		    return this;
	    }

	    public override bool Equals(Object o) 
        {
            Vec3f v = (Vec3f)o;
		    return (x == v.x && y == v.y);
	    }
	    public bool equals(int vx, int vy) {
		    return (x == vx && y == vy);
	    }

	    public String toString() {
		    return String.Format("{%d, %d}", x, y);
	    }

	    public static Vec2i Mul(Vec2i v, float s) {
		    return new Vec2i((int)(v.x * s), (int)(v.y * s));
	    }
	    public static Vec2i Mul(Vec2i v, Vec2i w) {
		    return new Vec2i(v.x * w.x, v.y * w.y);
	    }
	    public static Vec2i Sub(Vec2i a, Vec2i b) {
		    return new Vec2i(a.x - b.x, a.y - b.y);
	    }

	    public int hashCode() {
		    return x * 0xffff + y;
	    }
	    public bool equals(Object o) {
		    if (o is Vec2i) {
			    return equals((Vec2i)o);
		    }
		    return false;
	    }


	    public static Vec2i inter(Vec2i s, Vec2i e, float delta)
	    {
		    return new Vec2i(
			    (int)((e.x - s.x) * delta) + s.x,
			    (int)((e.y - s.y) * delta) + s.y);
	    }

	    // Animation

	    /*public Event<Vec2i> animateTo(Vec2i end, float delay, float duration)
	    {
		    return animateTo(end, delay, duration, Easings.Linear);
	    }
	    public Event<Vec2i> animateTo(Vec2i end, float delay, float duration, EasingMethod method)
	    {
		    return new Event<Vec2i>(this,
				    new Tween<Vec2i>(new Vec2i(this), new Vec2i(end)),
				    delay, duration, 0, 1, Easings.In, method);
	    }
        */
	    // Remote
	
	    public static int SIZE = 8;
	
	    
	    public bool IsEqual( Vec2i value ) {
		    return ( x == value.x && y == value.y );
	    }

	    
	    public void Copy( Vec2i from, Vec2i to ) {
		    to.x = from.x;
		    to.y = from.y;
	    }
	
	    
	    public Vec2i Clone() {
		    return new Vec2i( x, y );
	    }
	
	    
	    public void Update( Vec2i value ) {
		    value.x = x;
		    value.y = y;
	    }

	    
	    public void Scale( float d ) {
		    x *= (int)d;
            y *= (int)d;
	    }

	    
	    public Attribute<Vec2i> Create() {
		    return new Vec2i();
	    }

	    
	    public void write( MemoryStream memOut ) 
        {
            BinaryWriter writer = new BinaryWriter(memOut);
		    writer.Write( x );
            writer.Write(y);
	    }

	    
	    public void read( MemoryStream memIn ) 
        {
            BinaryReader reader = new BinaryReader( memIn );
		    x = reader.ReadInt32();
            y = reader.ReadInt32();
	    }

	    
	    public int bytes() {
		    return SIZE;
	    }

	    
	    public void read( InputModel input )
	    {
		    x = input.readInt( "x" );
		    y = input.readInt( "y" );
	    }

	    
	    public void write( OutputModel output )
	    {
		    output.write( "x", x );
		    output.write( "y", y );
	    }

    }
}
