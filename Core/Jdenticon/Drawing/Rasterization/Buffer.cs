#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    internal struct Buffer<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;

        public Buffer(int capacity)
        {
            items = new T[10];
            count = 0;
        }

        public void Initialize()
        {
            items = new T[10];
        }

        public void Add(T item)
        {
            if (count + 1 >= items.Length)
            {
                // Increase buffer
                var capacity = items.Length << 1;
                var oldEdges = items;
                items = new T[capacity];

                Array.Copy(oldEdges, 0, items, 0, count);
            }

            items[count] = item;
            count++;
        }

        public void Insert(int index, T item)
        {
            if (count + 1 >= items.Length)
            {
                // Increase buffer
                var capacity = items.Length << 1;
                var oldEdges = items;
                items = new T[capacity];

                Array.Copy(oldEdges, 0, items, 0, index);
                Array.Copy(oldEdges, index, items, index + 1, count - index);
            }
            else
            {
                ArrayUtils.ShiftRight(items, index, count - index);
            }

            items[index] = item;
            count++;
        }

        public void RemoveAt(int index)
        {
            ArrayUtils.ShiftLeft(items, index + 1, count - index - 1);
            count--;
        }
        
        public int Count => count;

        public ref T this[int index]
        {
            get { return ref items[index]; }
        }

        public void Clear()
        {
            count = 0;
        }

        public T[] GetBuffer()
        {
            return items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
