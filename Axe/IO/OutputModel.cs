using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.dreamwagon.axe
{
    public interface OutputModel
    {
	     String getName();
	     int getScope();
	     DataFormat getFormat();
	     OutputModel writeModel( String name );
	
	    // Attributes
	
	     void write(String name, Object value);
	     void writeClass(String name, Type type);
	     void writeType(String name, Type registryType, Object value);
	     void writeInstance(String name, OutputModel model, Type registryType);
	     void writeEnum(String name, Enum enumConstant);
	     void writeDelimited(String name, char delimiter, Object[] array);
	
	    // Children
	
	     OutputModel[] writeModelArray(String name, DataModel[] models );
	     OutputModel[] writeModelArray(String name, DataModel[] models, Type registryType, String attributeName );
	
	     OutputModel[] writeModelArrayQualified(String name, DataModel[] models, String className);
	
	     OutputModel[] writeModelArrayDynamic(String name, DataModel[] models, Type registryType, String attributeName );
	
	     OutputModel[] writeModelListQualified(String name, List<DataModel> models, String className);
	     OutputModel[] writeModelList(String name, List<DataModel> models);
	
	     OutputModel writeModel(String name, String className, DataModel model);
	
	     OutputModel writeModel(String name, DataModel model);
	     OutputModel writeModel(String name, DataModel model, Type registryType, String attributeName );

        List<OutputModel> writeModels(String name, OutputModelListener<OutputModel> listener, Enum[] models);
	
	    // Output
	
	    String toString();

        void output( StreamWriter writer);
	    void output( File file ) ;
	    void output( Stream stream ) ;
    }
}
