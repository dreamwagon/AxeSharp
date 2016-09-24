using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace com.dreamwagon.axe
{
    public class Reflections
    {

        public static T[] getStaticFieldArray<T>(Type type, Type from)
	    {
            List<T> fieldList = getStaticFieldCollection(type, from, new List<T>());

		    return fieldList.ToArray();
	    }

	    public static List<T> getStaticFieldCollection<T>( Type type, Type from, List<T> destination )
	    {
		    try
		    {
			    List<FieldInfo> fieldList = getStaticFields( type, from );

			    foreach (var f in fieldList)
			    {
				    destination.Add( (T)f.GetValue( null ) );
			    }
			
			    return destination;
		    }
		    catch (Exception e)
		    {
			    throw e;
		    }
	    }

	    public static Dictionary<String, T>  getStaticFieldMap<T>( Type type, Type from, Dictionary<String, T> destination )
	    {
		    try
		    {
			    List<FieldInfo> fieldList = getStaticFields( type, from );

			    foreach (var f in fieldList)
			    {
				    destination.Add( f.Name, (T)f.GetValue( null ) );
			    }

			    return destination;
		    }
		    catch (Exception e)
		    {
			    throw e;
		    }
	    }

	    public static List<FieldInfo> getStaticFields( Type type, Type from )
	    {
		    List<FieldInfo> fieldList = new List<FieldInfo>();
		    FieldInfo[] fieldArray = from.GetFields();

		    foreach (var f in fieldArray)
		    {
			    if ( !f.IsStatic)
			    {
				    continue;
			    }

			    if (!type.IsAssignableFrom( f.GetType() ))
			    {
				    continue;
			    }

			    fieldList.Add( f );
		    }

		    return fieldList;
	    }

    }
}
