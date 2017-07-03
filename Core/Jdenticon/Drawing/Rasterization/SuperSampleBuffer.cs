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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    /// <summary>
    /// A color buffer containing multiple samples per pixel.
    /// </summary>
    internal struct SuperSampleBuffer
    {
        private AverageColor[] buffer;
        private int bufferPixelOffset;
        private int bufferSampleOffset;

        public SuperSampleBuffer(int width)
        {
            this.buffer = new AverageColor[width];
            this.bufferPixelOffset = 0;
            this.bufferSampleOffset = 0;
        }

        public void Clear()
        {
            Rewind();

            for (var i = 0; i < buffer.Length && buffer[i].Count != 0; i++)
            {
                buffer[i].Clear();
            }
        }

        public void Rewind()
        {
            bufferPixelOffset = 0;
            bufferSampleOffset = 0;
        }

        public void CompletePixel(Color color)
        {
            if (bufferSampleOffset > 0)
            {
                buffer[bufferPixelOffset].Add(color, SuperSampling.SamplesPerPixelX - bufferSampleOffset);
                bufferSampleOffset = 0;
                bufferPixelOffset++;
            }
        }
        
        public void Add(Color color, float untilX)
        {
            var samplesLeft = (int)(untilX * SuperSampling.SamplesPerPixelX) - bufferPixelOffset * SuperSampling.SamplesPerPixelX - bufferSampleOffset;

            // First partial pixel
            if (bufferSampleOffset > 0)
            {
                var samples = SuperSampling.SamplesPerPixelX - bufferSampleOffset;
                if (samples > samplesLeft)
                {
                    samples = samplesLeft;
                }
                samplesLeft -= samples;

                buffer[bufferPixelOffset].Add(color, samples);

                bufferSampleOffset += samples;
                if (bufferSampleOffset == SuperSampling.SamplesPerPixelX)
                {
                    bufferSampleOffset = 0;
                    bufferPixelOffset++;
                }
            }

            // Full pixels
            var fullPixels = samplesLeft / SuperSampling.SamplesPerPixelX;
            if (fullPixels > 0)
            {
                for (var i = 0; i< fullPixels; i++)
                {
                    buffer[bufferPixelOffset + i].Add(color, SuperSampling.SamplesPerPixelX);
                }

                samplesLeft -= fullPixels * SuperSampling.SamplesPerPixelX;
                bufferPixelOffset += fullPixels;
            }

            // Last partial pixel
            if (samplesLeft > 0)
            {
                buffer[bufferPixelOffset].Add(color, samplesLeft);

                bufferSampleOffset += samplesLeft;

                if (bufferSampleOffset == SuperSampling.SamplesPerPixelX)
                {
                    bufferSampleOffset = 0;
                    bufferPixelOffset++;
                }
            }
        }

        public void WriteTo(ref BitmapWriter writer, int count)
        {
            for (var i = 0; i < count; i++)
            {
                writer.Write(buffer[i].Color);
            }
        }
    }
}
