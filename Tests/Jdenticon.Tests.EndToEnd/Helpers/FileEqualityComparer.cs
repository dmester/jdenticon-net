using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.Helpers
{
    public static class FileEqualityComparer
    {
        public static void AssertEqualImages(byte[] expected, byte[] actual)
        {
            using (Bitmap 
                bitmapExpected = (Bitmap)Image.FromStream(new MemoryStream(expected)), 
                bitmapActual = (Bitmap)Image.FromStream(new MemoryStream(actual)))
            {
                if (bitmapExpected.Width != bitmapActual.Width ||
                    bitmapExpected.Height != bitmapActual.Height)
                {
                    Assert.Fail("Bitmap dimension mismatch. Expected: <{0}, {1}>, Actual: <{2}, {3}>",
                        bitmapExpected.Width, bitmapExpected.Height,
                        bitmapActual.Width, bitmapActual.Height
                        );
                    return;
                }
                
                for (var y = 0; y < bitmapExpected.Height; y++)
                {
                    for (var x = 0; x < bitmapExpected.Width; x++)
                    {
                        if (bitmapExpected.GetPixel(x, y) != bitmapActual.GetPixel(x, y))
                        {
                            Assert.Fail("Bitmap difference found at ({0}, {1})", x, y);
                            return;
                        }
                    }
                }
            }
        }

        public static void AssertEqualBinary(byte[] expected, byte[] actual)
        {
            if (expected.Length != actual.Length)
            {
                Assert.Fail("Binary length mismatch. Expected: {0}, Actual: {1}", expected.Length, actual.Length);
                return;
            }

            for (var i = 0; i < expected.Length; i++)
            {
                if (expected[i] != actual[i])
                {
                    Assert.Fail("Binary difference found at offset {0}", i);
                    return;
                }
            }
        }

        public static void AssertEqualText(byte[] expected, byte[] actual)
        {
            using (TextReader
                readerExpected = new StringReader(Encoding.UTF8.GetString(expected)),
                readerActual = new StringReader(Encoding.UTF8.GetString(actual)))
            {
                var line = 0;

                while (true)
                {
                    var expectedLine = readerExpected.ReadLine();
                    var actualLine = readerActual.ReadLine();

                    line++;

                    if (expectedLine == null)
                    {
                        if (actualLine != null)
                        {
                            Assert.Fail("Expected end of file. Line: {1}", line);
                        }
                        return;
                    }

                    if (actualLine == null)
                    {
                        Assert.Fail("Did not expect end of file. Line: {1}", line);
                    }

                    var endIndex = Math.Min(expectedLine.Length, actualLine.Length);

                    for (var i = 0; i < endIndex; i++)
                    {
                        if (expectedLine[i] != actualLine[i])
                        {
                            Assert.Fail("Unexpected character at line {0}, column {1}", line, i + 1);
                            return;
                        }
                    }

                    if (expectedLine.Length > actualLine.Length)
                    {
                        Assert.Fail("Did not expect end of line. Line: {0}, Column: {1}", line, actualLine.Length);
                        return;
                    }

                    if (expectedLine.Length < actualLine.Length)
                    {
                        Assert.Fail("Expected end of line. Line: {0}, Column: {1}", line, expectedLine.Length);
                        return;
                    }
                }
            }
        }
    }
}
