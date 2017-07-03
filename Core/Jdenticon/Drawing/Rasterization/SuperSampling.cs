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
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    internal static class SuperSampling
    {
        // Note that the product of SamplesPerPixelX and SamplesPerPixelY
        // must not exceed 32 since AverageColor does not support more 
        // samples than that per pixel.

        public const int SamplesPerPixelX = 8;
        public const int SamplesPerPixelY = 3;

        public const float SampleWidth = 1f / SamplesPerPixelX;
        public const float SampleHeight = 1f / SamplesPerPixelY;
    }
}
