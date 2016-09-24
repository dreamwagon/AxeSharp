using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
public class Scalari : Attribute<Scalari> 
{
	public static int PI = (int)Math.PI;
	public static int PI2 = (int)(Math.PI * 2.0);

	public static Scalari ZERO = new Scalari( 0 );
	public static Scalari ONE = new Scalari( 1 );
	
	public int v;
	public Scalari() {
	}
	public Scalari(int v) {
		this.v = v;
	}
	public void Set(int v) {
		this.v = v;
	}
	public void Add(int s) {
		v += s;
	}
	public void Sub(int s) {
		v -= s;
	}
	public void Mul(int s) {
		v *= s;
	}
	public void Div(int s) {
		if (s != 0.0) {
			v /= s;
		}
	}
	public void max(int s) {
		v = (v > s ? s : v);
	}
	public void min(int s) {
		v = (v < s ? s : v);
	}
	public void clamp(int max, int min) {
		v = (v < min ? min : (v > max ? max : v));
	}
	public void delta(int start, int end, float delta) {
		v = (int)((end - start) * delta + start);
	}
	public void neg() {
		v = -v;
	}
	public void abs() {
		v = (v < 0 ? -v : v);
	}
	public void mod(int s) {
		v -= v % s;
	}
	public float cos() {
		return Numbers.COS[v % 360];
	}
	public float sin() {
		return Numbers.SIN[v % 360];
	}
	public float degrees() {
		return v;
	}
	public float radians() {
		return Numbers.RADIAN[v % 360];
	}
	public float Distance(Scalari value) {
		return Math.Abs(v - value.v);
	}
	public void Interpolate(Scalari start, Scalari end, float delta) {
		v = (int)((end.v - start.v) * delta + start.v);
	}
	public Scalari Get() {
		return this;
	}
	public void Set(Scalari value) {
		v = value.v;
	}
	public void Add(Scalari value, float scale) {
		v += (int)(value.v * scale);
	}
	public void max(Scalari max) {
		v = Math.Min(v, max.v);
	}

	public static int clamp(int v, int min, int max) {
		return (v < min ? min : (v > max ? max : v));
	}
	
	// Animation

	/*public Event<Scalari> animateTo(int end, float delay, float duration) 
	{
		return animateTo(end, delay, duration, Easings.Linear);
	}
	public Event<Scalari> animateTo(int end, float delay, float duration, EasingMethod method) 
	{
		return new Event<Scalari>(this, 
				new Tween<Scalari>(new Scalari(v), new Scalari(end)), 
				delay, duration, 0, 1, Easings.In, method);
	}*/
	
	// Remote
	
	public static  int SIZE = 4;
	
	public bool IsEqual( Scalari value ) {
		return ( v == value.v );
	}

	
	public void Copy( Scalari from, Scalari to ) {
		to.v = from.v;
	}
	
	
	public Scalari Clone() {
		return new Scalari( v );
	}
	
	
	public void Update( Scalari value ) {
		value.v = v;
	}
	
	
	public void Mul( Scalari value ) {
		v *= value.v;
	}
	
	
	public void Scale( float d ) {
		v *= (int)d;
	}
	
	
	public Attribute<Scalari> Create() {
		return new Scalari();
	}
	
	
	public void write( MemoryStream mem ) 
    {
		new BinaryWriter(mem).Write( v );
	}
	
	
	public void read( MemoryStream mem )
    {
        v = new BinaryReader(mem).ReadInt32();
	}
	
	
	public int bytes() {
		return SIZE;
	}
	
	
	public void read( InputModel input )
	{
		v = input.readInt( "v" );
	}
	
	
	public void write( OutputModel output )
	{
		output.write( "v", v );
	}
	
}
}
