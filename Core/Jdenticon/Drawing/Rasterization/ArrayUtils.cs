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

namespace Jdenticon.Drawing.Rasterization
{
    internal class ArrayUtils
    {
        public static void ShiftRight<T>(T[] array, int startIndex, int count)
        {
            if (count > 0)
            {
                var previousItem = array[startIndex];

                for (var i = 0; i < count; i++)
                {
                    var currentItem = array[startIndex + i + 1];
                    array[startIndex + i + 1] = previousItem;
                    previousItem = currentItem;
                }
            }
        }

        public static void ShiftLeft<T>(T[] array, int startIndex, int count)
        {
            if (count > 0)
            {
                var previousItem = array[startIndex];

                for (var i = 0; i < count; i++)
                {
                    array[startIndex + i - 1] = array[startIndex + i];
                }
            }
        }
    }
}