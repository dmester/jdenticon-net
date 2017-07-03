#region License
//
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// This software is provided 'as-is', without any express or implied
// warranty.In no event will the authors be held liable for any damages
// arising from the use of this software.
// 
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software.If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
//
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

        public void TrimEnd(Predicate<T> predicate)
        {
            for (var i = count - 1; i >= 0; i--)
            {
                if (!predicate(items[i]))
                {
                    count = i + 1;
                    return;
                }
            }

            count = 0;
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
