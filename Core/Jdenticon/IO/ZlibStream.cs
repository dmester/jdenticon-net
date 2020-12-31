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

#if USE_SHARP_COMPRESS
using SharpCompress.IO;
using SharpCompress.Compressors;
using SharpCompress.Compressors.Deflate;
#else 
using System.IO.Compression;
#endif

namespace Jdenticon.IO
{
    /// <summary>
    /// Wraps a Deflate stream in a zlib header and a Adler32 checksum according to https://tools.ietf.org/html/rfc1950.
    /// </summary>
    internal class ZlibStream : Stream
    {
        Adler32 adler32 = new Adler32();
        Stream outputStream;
        Stream deflateStream;

        public ZlibStream(Stream stream)
        {
            // CMF(Compression Method and flags)
            const byte cmf = 0x78;
            // CM (Compression method) = 8
            // CINFO (Compression info) = 32K window size

            // FLG (FLaGs)
            // FLEVEL = 2, compressor used default algorithm
            // FDICT = 0
            byte flg = 2 << 6;
            
            // FCHECK
            var mod = (cmf * 256 + flg % 31) % 31;
            if (mod != 0) flg += (byte)(31 - mod);

            stream.WriteByte(cmf);
            stream.WriteByte(flg);

            outputStream = stream;

#if USE_SHARP_COMPRESS
            deflateStream = new DeflateStream(new NonDisposingStream(stream),
                CompressionMode.Compress, CompressionLevel.Level3);
#elif HAVE_COMPRESSION_LEVEL
            deflateStream = new DeflateStream(stream, 
                CompressionLevel.Optimal, true);
#else
            deflateStream = new DeflateStream(new LeaveOpenStream(stream), CompressionMode.Compress);
#endif
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => deflateStream.Length;

        public override long Position { get => deflateStream.Position; set => throw new NotSupportedException(); }

        public override void Flush()
        {
            deflateStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            deflateStream.Write(buffer, offset, count);
            adler32.Update(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && deflateStream != null)
            {
                deflateStream.Flush();
                deflateStream.Dispose();
                deflateStream = null;

                outputStream.WriteBigEndian(adler32.Value);
            }

            base.Dispose(disposing);
        }
    }
}
