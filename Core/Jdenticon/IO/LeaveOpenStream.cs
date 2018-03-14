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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.IO
{
    /// <summary>
    /// Proxies all calls to the base stream but leaves the base stream
    /// open when the <see cref="LeaveOpenStream"/> is disposed.
    /// </summary>
    internal class LeaveOpenStream : Stream
    {
        private readonly Stream baseStream;

        public LeaveOpenStream(Stream baseStream)
        {
            this.baseStream = baseStream;
        }

        public override bool CanRead => baseStream.CanRead;

        public override bool CanSeek => baseStream.CanSeek;

        public override bool CanWrite => baseStream.CanWrite;

        public override long Length => baseStream.Length;

        public override long Position
        {
            get => baseStream.Position;
            set => baseStream.Position = value;
        }

        public override void Flush()
            => baseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
            => baseStream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin)
            => baseStream.Seek(offset, origin);

        public override void SetLength(long value)
            => baseStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
            => baseStream.Write(buffer, offset, count);
    }
}
