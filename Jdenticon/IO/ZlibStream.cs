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

#if USE_SHARP_COMPRESS
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
            deflateStream = new DeflateStream(stream, 
                CompressionMode.Compress, CompressionLevel.Level3, true);
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
