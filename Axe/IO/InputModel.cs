using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public interface InputModel : IEnumerable<InputModel>, Asset
    {
	    String getName();
	    DataFormat getFormat();
	
	    void copyTo( OutputModel outModel );
	
	    InputModel[] getChildren( String name );
	
	    bool hasAttribute(String name);
	
	    // Attributes
	
	     double readDouble(String name);
	     double readDouble(String name, Double defaultValue);
	     float readFloat(String name);
	     float readFloat(String name, float defaultValue);
	     byte readByte(String name);
	     byte readByte(String name, Byte defaultValue);
	     short readShort(String name);
	     short readShort(String name, short defaultValue);
	     int readInt(String name) ;
	     int readInt(String name, int defaultValue) ;
	     bool readBoolean(String name) ;
	     bool readBoolean(String name, Boolean defaultValue);
	     char readChar(String name) ;
	     char readChar(String name, char defaultValue);
	     String readString(String name) ;
	     String readString(String name, String defaultValue);
	     String readNullableString(String name) ;
	     Type readClass(String name) ;
	     Type readClass(String name, Type defaultClass) ;
	     Type readType(String name, Type registryType) ;
	     Type readInstance(String name, Type registryType) ;
	     Type readInstance(String className) ;
	     Enum readEnum(String name, Enum enumType) ;
	     Enum readEnum(String name, Type enumType, Enum enumConstant) ;
	     Type[] readDelimited(String name, char delimiter, Type itemType) ;

	    // Children
	
	     Type[] readModelArray(String name, Type itemType) ;
	     Type[] readModelArray(String name, Type registryType, String attributeName) ;
	     InputModel[] readModelArray(String name) ;

	     Type[] readModelArrayQualified(String name, Type itemType, String className) ;
	     Type[] readModelArrayDynamic(String name, Type registryType, String attributeName ) ;
	
	     List<Type> readModelList(String name, Type itemType) ;
	     List<Type> readModelList(String name, Type registryType, String attributeName) ;
	     List<InputModel> readModelList(String name) ;
	     List<Type> readModelListQualified(String name, String className) ;
	
	     Type readModel(String name, String className) ;
	
	     Type readModel(String name, Type model) ;
	     InputModel readModel(String name) ;
	
	     Type readModel(String name, Type registryType, String attributeName, bool requires) ;
	
	     List<Type> readModels(String name, InputModelListener listener) ;
    }
}
