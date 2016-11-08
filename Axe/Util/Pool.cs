using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.dreamwagon.axe
{
    public delegate T Factory<T>();

    public class Pool<T>
    {

        public int size;
        public T[] stack;
        public Factory<T> factory;

        public Pool(T[] stack, Factory<T> factory)
        {
            this.stack = stack;
            this.factory = factory;
        }
        public T alloc()
        {
            return (size == 0 ? factory() : stack[--size]);
        }
        public void free(T item)
        {
            if (size < stack.Length)
            {
                stack[size++] = item;
            }
        }
    }
}
