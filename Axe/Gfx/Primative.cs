using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
public class Primitive : Cloned<Primitive>
{

	public static Primitive Point = new Primitive( 0, false );
	public static Primitive Line = new Primitive( 1, false );
	public static Primitive LineStrip = new Primitive( 2, false );
	public static Primitive LineLoop = new Primitive( 3, false );
	public static Primitive Triangle = new Primitive( 4, true );
	public static Primitive TriangleStrip = new Primitive( 5, true );
	public static Primitive TriangleFan = new Primitive( 6, true );
	public static Primitive Quad = new Primitive( 7, true );
	public static Primitive QuadStrip = new Primitive( 8, true );
	public static Primitive Polygon = new Primitive( 9, true );

	public int x;
	public bool texture;

	private Primitive( int x, bool texture )
	{
		this.x = x;
		this.texture = texture;
	}

	public Primitive clone() {
		return this; // immutable
	}

}
}
