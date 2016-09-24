using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace com.dreamwagon.axe
{
    public class Camera3
    {
	    public Scalarf yaw = new Scalarf();
        public Scalarf pitch = new Scalarf();
	    public Scalarf roll = new Scalarf();
	    public Scalarf distance = new Scalarf();
	    public Vec3f focus = new Vec3f();
	    public Vec3f direction = new Vec3f();  //<auto>
	    public Vec3f right = new Vec3f(); //<auto>
	    public Vec3f up = new Vec3f(); //<auto>
	    public Vec3f position = new Vec3f(); //<auto>
	    public Vec3f forward = new Vec3f(); //<auto>
	    public Quaternion3 rollq = new Quaternion3(); // <auto>

	    public Camera3()
	    {
	    }

	    public Camera3(Camera3 c)
	    {
		    yaw.Set( c.yaw );
		    pitch.Set( c.pitch );
		    roll.Set( c.roll );
		    distance.Set( c.distance );
		    focus.Set( c.focus );
	    }

	    public void init() {

	    }

        public void validate()
        {
            // Wrap all angles between 0 and 2PI
            yaw.Mod(Scalarf.PI2);
            pitch.Mod(Scalarf.PI2);
            roll.Mod(Scalarf.PI2);

            // Setup the camera planes
        }

	    public Camera3 Get()
	    {
		    return this;
	    }

	
	    public void Set(Camera3 target)
	    {
            yaw.Set(target.yaw);
            pitch.Set(target.pitch);
            roll.Set(target.roll);
            focus.Set(target.focus);
            distance.Set(target.distance);
	    }

	
	    public void Interpolate(Camera3 start, Camera3 end, float delta)
	    {
            yaw.Interpolate(start.yaw, end.yaw, delta);
            pitch.Interpolate(start.pitch, end.pitch, delta);
            roll.Interpolate(start.roll, end.roll, delta);
            focus.Interpolate(start.focus, end.focus, delta);
            distance.Interpolate(start.distance, end.distance, delta);
	    }

	
	    public void Add( Camera3 value, float delta )
	    {
            yaw.Add(value.yaw, delta);
            pitch.Add(value.pitch, delta);
		    roll.Add (value.roll, delta );
		    focus.Add( value.focus, delta );
		    distance.Add( value.distance, delta );
	    }

	
	    public float Distance(Camera3 to)
	    {
		    return position.Distance(to.position);
	    }

	    // Remote

        public static int SIZE = 28;
	
	
	    public void copy( Camera3 from, Camera3 to ) {
		    to.Set( from );
	    }
	
	
	    public Camera3 clone() {
		    return new Camera3( this );
	    }

	
	    public void update( Camera3 value ) {
		    value.Set( this );
	    }


	
	    public void Mul( Camera3 value ) {
		    yaw.Mul( value.yaw );
		    pitch.Mul( value.pitch );
		    roll.Mul( value.roll );
		    distance.Mul( value.distance );
		    focus.Mul( value.focus );
	    }

	
	    public void Scale( float d ) {
		    yaw.Scale( d );
		    pitch.Scale( d );
		    roll.Scale( d );
		    distance.Scale( d );
		    focus.Scale( d );
	    }
	
    }
}
