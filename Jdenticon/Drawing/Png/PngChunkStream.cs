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

using Jdenticon.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.Drawing.Png
{
    /// <summary>
    /// A stream for writing the data of an individual PNG chunk.
    /// </summary>
    internal class PngChunkStream : Stream
    {
        private readonly Stream outputStream;
        private readonly MemoryStream buffer;
        private readonly string name;

        public PngChunkStream(Stream outputStream, string name)
        {
            this.outputStream = outputStream;
            this.name = name;
            this.buffer = new MemoryStream();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                var data = buffer.GetBuffer();
                var crc = new Crc32();

                // Length 32
                outputStream.WriteBigEndian((int)buffer.Length);

                // Name
                var binaryName = Encoding.UTF8.GetBytes(name);
                outputStream.Write(binaryName, 0, binaryName.Length);
                crc.Update(binaryName, 0, binaryName.Length);

                // Data
                outputStream.Write(data, 0, (int)buffer.Length);
                crc.Update(data, 0, (int)buffer.Length);
                
                // crc32: type + data
                outputStream.WriteBigEndian(crc.Value);
            }
        }
        
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => buffer.Length;

        public override long Position { get => buffer.Position; set => buffer.Position = value; }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.buffer.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return buffer.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            buffer.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.buffer.Write(buffer, offset, count);
        }

        public void Write(string s)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(s);
            this.buffer.Write(bytes, 0, bytes.Length);
        }
    }
}
