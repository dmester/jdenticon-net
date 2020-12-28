using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jdenticon.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jdenticon.Tests
{
    [TestClass]
    public class IdenticonExtensionTests
    {
        private void AssertEqualCore(string test, Stream expected, Stream actual)
        {
            expected.Position = 0;

            Assert.AreEqual(expected.Length, actual.Length, $"{test}: Length diff");

            var expectedBuffer = new byte[expected.Length];
            var actualBuffer = new byte[actual.Length];

            expected.Read(expectedBuffer, 0, expectedBuffer.Length);
            actual.Read(actualBuffer, 0, actualBuffer.Length);

            for (var i = 0L; i < expected.Length; i++)
            {
                Assert.AreEqual(expectedBuffer[i], actualBuffer[i], $"{test}: Content diff");
            }
        }

        private void AssertEqual(string test, Stream expected, Stream actual)
        {
            actual.Position = 0;
            AssertEqualCore(test, expected, actual);
        }

        private void AssertEqualRewinded(string test, Stream expected, Stream actual)
        {
            Assert.AreEqual(0, actual.Position);
            Assert.IsTrue(actual.CanRead);
            Assert.IsTrue(actual.CanSeek);
            Assert.IsFalse(actual.CanWrite);
            AssertEqualCore(test, expected, actual);
        }

        private void AssertEqualStream(string test, Stream expected, Action<Stream> callback)
        {
            var actual = new MemoryStream();
            callback(actual);
            Assert.AreEqual(actual.Length, actual.Position);
            AssertEqual(test, expected, actual);
        }

        private void AssertEqualFile(string test, Stream expected, Action<string> callback)
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                callback(tempFile);

                using (var actual = File.OpenRead(tempFile))
                {
                    AssertEqual(test, expected, actual);
                }
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private void AssertEqualWriter(string test, Stream expected, Action<TextWriter> callback)
        {
            using (var actual = new MemoryStream())
            {
                using (var writer = new StreamWriter(actual, Encoding.UTF8))
                {
                    callback(writer);
                    writer.Flush();

                    AssertEqual(test, expected, actual);
                }
            }
        }

        private void AssertEqualStream(string test, Stream expected, Func<Stream, Task> callback)
        {
            var actual = new MemoryStream();
            callback(actual).Wait();
            Assert.AreEqual(actual.Length, actual.Position);
            AssertEqual(test, expected, actual);
        }

        private void AssertEqualFile(string test, Stream expected, Func<string, Task> callback)
        {
            var tempFile = Path.GetTempFileName();
            try
            {
                callback(tempFile).Wait();

                using (var actual = File.OpenRead(tempFile))
                {
                    AssertEqual(test, expected, actual);
                }
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        private void AssertEqualWriter(string test, Stream expected, Func<TextWriter, Task> callback)
        {
            using (var actual = new MemoryStream())
            {
                using (var writer = new StreamWriter(actual, Encoding.UTF8))
                {
                    callback(writer).Wait();
                    writer.Flush();

                    AssertEqual(test, expected, actual);
                }
            }
        }

        [TestMethod]
        public void Identicon_Svg()
        {
            var icon = Identicon.FromValue("Jdenticon", 100);

            var expected = icon.SaveAsSvg();

            AssertEqualRewinded("SaveAsSvg()", expected, icon.SaveAsSvg());
            AssertEqualRewinded("SaveAsSvg(false)", expected, icon.SaveAsSvg(false));
            AssertEqualStream("SaveAsSvg(stream)", expected, stream => icon.SaveAsSvg(stream));
            AssertEqualWriter("SaveAsSvg(writer)", expected, writer => icon.SaveAsSvg(writer));
            AssertEqualFile("SaveAsSvg(path)", expected, path => icon.SaveAsSvg(path));
            AssertEqualStream("SaveAsSvg(stream, false)", expected, stream => icon.SaveAsSvg(stream, false));
            AssertEqualWriter("SaveAsSvg(writer, false)", expected, writer => icon.SaveAsSvg(writer, false));
            AssertEqualFile("SaveAsSvg(path, false)", expected, path => icon.SaveAsSvg(path, false));
            AssertEqualStream("SaveAsSvgAsync(stream)", expected, stream => icon.SaveAsSvgAsync(stream));
            AssertEqualWriter("SaveAsSvgAsync(writer)", expected, writer => icon.SaveAsSvgAsync(writer));
            AssertEqualFile("SaveAsSvgAsync(path)", expected, path => icon.SaveAsSvgAsync(path));
            AssertEqualStream("SaveAsSvgAsync(stream, false)", expected, stream => icon.SaveAsSvgAsync(stream, false));
            AssertEqualWriter("SaveAsSvgAsync(writer, false)", expected, writer => icon.SaveAsSvgAsync(writer, false));
            AssertEqualFile("SaveAsSvgAsync(path, false)", expected, path => icon.SaveAsSvgAsync(path, false));

            var expectedFragment = icon.SaveAsSvg(true);

            AssertEqualRewinded("SaveAsSvg(true)", expectedFragment, icon.SaveAsSvg(true));
            AssertEqualStream("SaveAsSvg(stream, true)", expectedFragment, stream => icon.SaveAsSvg(stream, true));
            AssertEqualWriter("SaveAsSvg(writer, true)", expectedFragment, writer => icon.SaveAsSvg(writer, true));
            AssertEqualFile("SaveAsSvg(path, true)", expectedFragment, path => icon.SaveAsSvg(path, true));
            AssertEqualStream("SaveAsSvgAsync(stream, true)", expectedFragment, stream => icon.SaveAsSvgAsync(stream, true));
            AssertEqualWriter("SaveAsSvgAsync(writer, true)", expectedFragment, writer => icon.SaveAsSvgAsync(writer, true));
            AssertEqualFile("SaveAsSvgAsync(path, true)", expectedFragment, path => icon.SaveAsSvgAsync(path, true));
        }

        [TestMethod]
        public void Identicon_Png()
        {
            var icon = Identicon.FromValue("Jdenticon", 100);

            var expected = icon.SaveAsPng();

            AssertEqualRewinded("SaveAsPng()", expected, icon.SaveAsPng());
            AssertEqualStream("SaveAsPng(stream)", expected, stream => icon.SaveAsPng(stream));
            AssertEqualFile("SaveAsPng(path)", expected, path => icon.SaveAsPng(path));
            AssertEqualStream("SaveAsPngAsync(stream)", expected, stream => icon.SaveAsPngAsync(stream));
            AssertEqualFile("SaveAsPngAsync(path)", expected, path => icon.SaveAsPngAsync(path));
        }


        [TestMethod]
        public void Identicon_Emf()
        {
            var icon = Identicon.FromValue("Jdenticon", 100);

            // Dry run since the first file seems to be a little different
            icon.SaveAsEmf();

            var expected = icon.SaveAsEmf();
            
            AssertEqualRewinded("SaveAsEmf()", expected, icon.SaveAsEmf());
            AssertEqualStream("SaveAsEmf(stream)", expected, stream => icon.SaveAsEmf(stream));
            AssertEqualFile("SaveAsEmf(path)", expected, path => icon.SaveAsEmf(path));
            AssertEqualStream("SaveAsEmfAsync(stream)", expected, stream => icon.SaveAsEmfAsync(stream));
            AssertEqualFile("SaveAsEmfAsync(path)", expected, path => icon.SaveAsEmfAsync(path));
        }
    }
}
