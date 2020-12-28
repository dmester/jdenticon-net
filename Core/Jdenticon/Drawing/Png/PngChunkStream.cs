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
#if HAVE_MEMORYSTREAM_GETBUFFER
                var data = buffer.GetBuffer();
#else
                var data = buffer.ToArray();
#endif

                var dataLength = (int)buffer.Length;
                var crc = new Crc32();

                // Length 32
                outputStream.WriteBigEndian(dataLength);

                // Name
                var binaryName = Encoding.UTF8.GetBytes(name);
                outputStream.Write(binaryName, 0, binaryName.Length);
                crc.Update(binaryName, 0, binaryName.Length);

                // Data
                outputStream.Write(data, 0, dataLength);
                crc.Update(data, 0, dataLength);
                
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
