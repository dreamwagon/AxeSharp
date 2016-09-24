using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{

    #region Easing Implementation
    public class In : AbstractEasingType
    {
        public In() : base("In"){}
        public override float delta(float d, EasingMethod f)
        {
			return f.motion(d);
		}
	}

    public class Out : AbstractEasingType
    {
        public Out() : base("Out"){}
        public override float delta(float d, EasingMethod f)
        {
			return (1 - f.motion(1 - d));
		}
	}

    public class InOut : AbstractEasingType
    {
        public InOut() : base("InOut"){}
        public override float delta(float d, EasingMethod f)
        {
			if (d < 0.5)
				return f.motion(2 * d) * 0.5f;
	
			return (1 - f.motion(2 - 2 * d) * 0.5f);
		}
	}

    public class PingPong : AbstractEasingType
    {
        public PingPong() : base("PingPong"){}
        public override float delta(float d, EasingMethod f)
        {
			if (d < 0.5)
				return f.motion(2 * d);
			return f.motion(2 - 2 * d);
		}
	}
    #endregion


	public class Linear : AbstractEasingMethod
    {
        public Linear() : base( "Linear" ) {}
        public override float motion(float d)
        {
			return d;
		}
	};

    public class Quadratic : AbstractEasingMethod
    {
        public Quadratic() : base( "Quadratic" ) {}
        public override float motion(float d)
        {
			return d * d;
		}
	};

    public class Cubic : AbstractEasingMethod
    {
        public Cubic() : base( "Cubic" ) {}
        public override float motion(float d)
        {
			return d * d * d;
		}
	};

    public class Quartic : AbstractEasingMethod
    {
        public Quartic() : base( "Quartic" ) {}
        public override float motion(float d)
        {
			float d2 = d * d;
			return d2 * d2;
		}
	};

    public class Quintic : AbstractEasingMethod
    {
        public Quintic() : base( "Quintic" ) {}
        public override float motion(float d)
        {
			float d2 = d * d;
			return d2 * d2 * d;
		}
	};

    public class Back : AbstractEasingMethod
    {
        public Back() : base( "Back" ) {}
		public override float motion(float d) {
			float d2 = d * d;
			float d3 = d2 * d;
			return d3 + d2 - d;
		}
	};

    public class Sine : AbstractEasingMethod
    {
        public Sine() : base( "Sine" ) {}
		private double FREQUENCY = Math.PI * 0.5;
		public override float motion(float d) {
			return (float)Math.Sin(d * FREQUENCY);
		}
	};

    public class Easings
    {
        //Easing types
	    public static EasingType In = new In();
	    public static EasingType Out = new Out();
	    public static EasingType InOut = new InOut();
	    public static EasingType PingPong = new PingPong();

        //Easing methods
	    public static EasingMethod Linear = new Linear();
	    public static EasingMethod Quadratic = new Quadratic();
	    public static EasingMethod Cubic = new Cubic();
	    public static EasingMethod Quartic = new Quartic();
	    public static EasingMethod Quintic = new Quintic();
	    public static EasingMethod Back = new Back();
	    public static EasingMethod Sine = new Sine();

	    /* public static EasingMethod Elastic = new AbstractEasingMethod( "Elastic" ) {
		    private double FREQUENCY = Math.PI * 3.5;
		    public float motion(float d) {
			    float d2 = d * d;
			    float d3 = d2 * d;
			    float scale = d2 * ((2 * d3) + d2 - (4 * d) + 2);
			    float wave = -(float)Math.sin(d * FREQUENCY);
			    return scale * wave;
		    }
	    };
	    public static EasingMethod Revisit = new AbstractEasingMethod( "Revisit" ) {
		    private double FREQUENCY = Math.PI;
		    public float motion(float d) {
			    return (float)Math.abs(-Math.sin(d * FREQUENCY) + d);
		    }
	    };
	    public static EasingMethod SlowBounce = new AbstractEasingMethod( "SlowBounce" ) {
		    private double FREQUENCY = Math.PI * Math.PI * 1.5;
		    public float motion(float d) {
			    float d2 = d * d;
			    return (float)(1 - Math.abs((1 - d2) * Math.cos(d2 * d * FREQUENCY)));
		    }
	    };
	    public static EasingMethod Bounce = new AbstractEasingMethod( "Bounce" ) {
		    private double FREQUENCY = Math.PI * Math.PI * 1.5;
		    public float motion(float d) {
			    return (float)(1 - Math.abs((1 - d) * Math.cos(d * d * FREQUENCY)));
		    }
	    };
	    public static EasingMethod SmallBounce = new AbstractEasingMethod( "SmallBounce" ) {
		    private double FREQUENCY = Math.PI * Math.PI * 1.5;
		    public float motion(float d) {
			    float inv = 1 - d;
			    return (float)(1 - Math.abs(inv * inv * Math.cos(d * d * FREQUENCY)));
		    }
	    };
	    public static EasingMethod TinyBounce = new AbstractEasingMethod( "TinyBounce" ) {
		    private double FREQUENCY = 7;
		    public float motion(float d) {
			    float inv = 1 - d;
			    return (float)(1 - Math.abs(inv * inv * Math.cos(d * d * FREQUENCY)));
		    }
	    };
	    public static EasingMethod Hesitant = new AbstractEasingMethod( "Hesitant" ) {
		    public float motion(float d) {
			    return (float)(Math.cos(d * d * 12) * d * (1 - d) + d);
		    }
	    };
	    public static EasingMethod Lasso = new AbstractEasingMethod( "Lasso" ) {
		    public float motion(float d) {
			    float d2 = d * d;
			    return (float)(1 - Math.cos(d2 * d * 36) * (1 - d));
		    }
	    };
	    public static EasingMethod Sqrt = new AbstractEasingMethod( "Sqrt" ) {
		    public float motion(float d) {
			    return (float)Math.sqrt(d);
		    }
	    };
	    public static EasingMethod Log10 = new AbstractEasingMethod( "Log10" ) {
		    public float motion(float d) {
			    return (float)((Math.log10(d) + 2) * 0.5);
		    }
	    };
	    public static EasingMethod Slingshot = new AbstractEasingMethod( "Slingshot" ) {
		    public float motion(float d) {
			    if (d < 0.7f)
				    return (d * -0.357f);
	
			    float x = d - 0.7f;
			    return ((x * x * 27.5f - 0.5f) * 0.5f);
		    }
	    };
	    public static EasingMethod Circlular = new AbstractEasingMethod( "Circular" ) {
		    public float motion(float d) {
			    return 1 - (float)Math.sqrt( 1 - d * d );
		    }
	    };
	    public static EasingMethod Gentle = new AbstractEasingMethod( "Gentle" ) {
		    public float motion( float d ) {
			    return (3 * (1 - d) * d * d) + (d * d * d);
		    }
	    }; */

	    public static Easing Default = new Easing( In, Linear );

	    public static EasingType[] Types = Reflections.getStaticFieldArray<EasingType>( typeof(EasingType), typeof(Easings) );
	
	    public static Dictionary<String, EasingType> TypeMap = Reflections.getStaticFieldMap(  typeof(EasingType), typeof(Easings), new Dictionary<String, EasingType>() );
	
	    public static EasingMethod[] Methods = Reflections.getStaticFieldArray<EasingMethod>( typeof(EasingMethod), typeof(Easings) );

	    public static Dictionary<String, EasingMethod> MethodMap = Reflections.getStaticFieldMap( typeof(EasingMethod), typeof(Easings), new Dictionary<String, EasingMethod>() );
	
    }
}
