using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public class Array 
    {
	    public static T[] fromCollection<T>( List<T> items ) 
	    {
		    return items.ToArray<T>();
	    }

	    public static T[] add<T>(T e, T[] elements)
	    {
		    int size = elements.Length;
            System.Array.Copy( elements, elements, size + 1);
		    elements[size] = e;
		    return elements;
	    }
	
	    public static float[] add(float e, float[] elements)
	    {
		    int size = elements.Length;
		    System.Array.Copy( elements, elements, size + 1);
		    elements[size] = e;
		    return elements;
	    }
	
	    public static int[] add(int e, int[] elements)
	    {
		    int size = elements.Length;
		    System.Array.Copy( elements, elements, size + 1);
		    elements[size] = e;
		    return elements;
	    }
	
	    public static bool[] add(bool e, bool[] elements)
	    {
		    int size = elements.Length;
		    System.Array.Copy( elements, elements, size + 1);
		    elements[size] = e;
		    return elements;
	    }
	
	    /*public static void insert<T>( int index, T e, T[] elements )
	    {
		     System.arraycopy( elements, index, elements, index + 1, elements.Length - index );
		 
		     elements[ index ] = e;
	    }*/
	
	   /* public static T remove<T>( int index, T[] elements )
	    {
		    T e = elements[ index ];
		
		    System.arraycopy( elements, index - 1, elements, index, elements.Length - index );
		
		    return e;
	    }*/
	
	    public static int indexOf<T>(T e, T[] elements)
	    {
		    for (int i = 0; i < elements.Length; i++) {
			    if (elements[i].Equals( e )) {
				    return i;
			    }
		    }
		    return -1;
	    }
	
	    public static T[] remove<T>(T e, T[] elements) 
	    {
		    int i = indexOf(e, elements);
		    if (i >= 0) {
			    int size = elements.Length - 1;
			    elements[i] = elements[size];
			    System.Array.Copy( elements, elements, size);
		    }
		    return elements;
	    }
	
	    public static Attribute<T>[] copy<T>(Attribute<T>[] inT, Attribute<T>[] outT)
	    {
		    for (int i = 0; i < outT.Length; i++)
		    {
			    if (outT[i] == null)
			    {
                    outT[i] = (Attribute<T>)inT[i].Clone();
			    }
			    else
			    {
                    outT[i].Set( (T)inT[i] );	
			    }
		    }
		
		    return outT;
	    }
	
	    public static T[] select<T>( T[] elements, int[] indices )
	    {
		    T[] copy = new T[indices.Length];
            System.Array.Copy( copy , elements, indices.Length);
		    for (int i = 0; i < indices.Length; i++) 
		    {
			    copy[i] = elements[ indices[i] ];
		    }
		
		    return copy;
	    }
	
	    public static T[] select<T>( T[] elements, Enum[] enumConstants )
	    {
		    T[] copy = new T[enumConstants.Length];
            System.Array.Copy( copy , elements, enumConstants.Length);

		    /* for (int i = 0; i < enumConstants.Length; i++) 
		    {
			    copy[i] = elements[ enumConstants[i]. ];
		    }*/
		
		    return copy;
	    }
	
	    public static T[] selectRanges<T>( T[] elements, int[] indices )
	    {
		    int total = 0, current = 0;
		    int[] dir = new int[ indices.Length >> 1 ];
		
		    for ( int i = 0; i < indices.Length; i+=2 )
		    {
			    int k = i >> 1;
			    int d = indices[ i + 1 ] - indices[ i ];
                dir[k] = Math.Sign(d);
			    total += d;
		    }
		
		    T[] copy = new T[ total ];
            System.Array.Copy(copy, elements, total);

		    for ( int i = 0; i < indices.Length; i+=2 )
		    {
			    for (int k = indices[i]; k != indices[i + 1]; k += dir[ i >> 1 ] ) 
			    {
				    copy[ current++ ] = elements[ k ];
			    }
			    copy[ current++ ] = elements[ indices[i + 1] ];
		    }
		
		    return copy;
	    }

	    /*public static T[] allocate<T>( T[] array, ArrayAllocator<T> allocator )
	    {
		    for (int i = 0; i < array.Length; i++)
		    {
			    array[i] = allocator.allocate( i );
		    }
		    return array;
	    }*/
	
	    public static T[] allocate<T>( int size, Attribute<T> template )
	    {
		    return allocate ( new T[size], template );
	    }
	
	    public static T[] allocate<T>( T[] array, Attribute<T> template )
	    {
		    for (int i = 0; i < array.Length; i++)
		    {
                array[i] = template.Clone();
		    }
		
		    return array;
	    }
	
	    public static T[] allocate<T>( int size, Factory<T> factory )
	    {
		    return allocate( new T[size], factory );
	    }
	
	    public static T[] allocate<T>( T[] array, Factory<T> factory )
	    {
		    for (int i = 0; i < array.Length; i++)
		    {
			    array[i] = factory();
		    }
		
		    return array;
	    }
	
	    public static T[] place<T>(int relativeIndex, T value, T[] array)
	    {
		    if (relativeIndex < 0) 
		    {
                System.Array.Copy(array, array, array.Length - relativeIndex);
                System.Array.Copy(array, 0, array, -relativeIndex, array.Length + relativeIndex);
			    Fill( array, 0, -relativeIndex - 1, default(T) );
			    array[0] = value;
		    }
		    else if (relativeIndex >= array.Length)
		    {
                System.Array.Copy(array, array, relativeIndex + 1);
			    array[relativeIndex] = value;
		    }
		    else
		    {
			    array[relativeIndex] = value;
		    }
		
		    return array;
	    }
	
	    public static T[] trim<T>(T[] array)
	    {
		    int tn = trailingNulls( array );
		
		    if (tn > 0) {
			    System.Array.Copy( array, array, array.Length - tn );
		    }
		
		    int ln = leadingNulls( array );
		
		    if (ln > 0) {
			    int z = array.Length - ln;
                System.Array.Copy(array, ln, array, 0, z);
                System.Array.Copy(array, array, z);
		    }
		
		    return array;
	    }
	
	    public static int leadingNulls<T>(T[] input)
	    {
		    int nullCount = 0;

		    while (nullCount < input.Length && input[nullCount] == null) 
		    {
			    nullCount++;
		    }
		
		    return nullCount;
	    }
	
	    public static int trailingNulls<T>(T[] input)
	    {
		    int n = input.Length;
		    int nullCount = 0;
		
		    while (nullCount < input.Length && input[--n] == null) 
		    {
			    nullCount++;
		    }
		
		    return nullCount;
	    }

        public static void Fill<T>(T[] array, int start, int end, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (start < 0 || start >= end)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            if (end >= array.Length)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

    }

}
