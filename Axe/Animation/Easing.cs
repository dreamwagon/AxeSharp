using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class Easing : DataModel
    {
	    private EasingType type;
	    private EasingMethod method;
	    private float scale;

	    public Easing() : this( Easings.In, Easings.Linear, 1f ){}

	    public Easing( Easing e ) : this( e.type, e.method, e.scale ){}
	
	    public Easing( EasingType type, EasingMethod method ) :  this( type, method, 1f ){}

	    public Easing( EasingType type, EasingMethod method, float scale )
	    {
		    this.type = type;
		    this.method = method;
		    this.scale = scale;
	    }

	    public float delta( float delta )
	    {
		    float d = type.delta( delta, method );
		    if (scale != 1f)
		    {
			    d = scale * d + (1 - scale) * delta;
		    }
		    return d;
	    }

	    public EasingType Type()
	    {
		    return type;
	    }

	    public void Type( EasingType type )
	    {
		    this.type = type;
	    }

	    public EasingMethod Method()
	    {
		    return method;
	    }

	    public void Method( EasingMethod method )
	    {
		    this.method = method;
	    }

	    public float Scale()
	    {
		    return scale;
	    }

	    public void Scale( float scale )
	    {
		    this.scale = scale;
	    }

	    public void write( OutputModel output )
	    {
		    output.write( "scale", scale );
		    output.write( "type", type.Name() );
		    output.write( "method", method.Name() );
	    }

	    public void read( InputModel input )
	    {
		    scale = input.readFloat( "scale" );
		    Easings.TypeMap.TryGetValue( input.readString( "type" ), out type );
            Easings.MethodMap.TryGetValue(input.readString("method"), out method);
	    }

    }

}
