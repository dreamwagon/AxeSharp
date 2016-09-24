using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
    public interface DataFormat
    {
	     OutputModel newOutput( String name );

	     OutputModel newOutput( String name, DataModel model );
	
	     OutputModel newOutput( String name, int scope );

	     OutputModel newOutput( String name, int scope, DataModel model );

         OutputModel write( StreamWriter writer, String name, DataModel model);

	     OutputModel write( File file, String name, DataModel model );

	     OutputModel write( Stream stream, String name, DataModel model );
	
	     InputModel read( Stream stream );

	     InputModel read( StreamReader reader );

	     InputModel read( File file );

	     InputModel read( String str );

	     DataModel read( Stream stream, DataModel model );

	     DataModel  read( StreamReader reader, DataModel model );

	     DataModel  read( File file, DataModel model );

	     DataModel  read( String str, DataModel model );

    }
}
