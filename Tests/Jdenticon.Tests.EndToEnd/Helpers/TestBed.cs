using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.Helpers
{
    public class TestBed : IDisposable
    {
        private readonly Type testType;
        private readonly string actualResultBasePath;
        
        protected TestBed(Type testType, TestContext context)
        {
            this.testType = testType;

            actualResultBasePath = Path.Combine(context.TestRunResultsDirectory, "ActualResults", testType.Name);

            Directory.CreateDirectory(actualResultBasePath);
        }

        public static WebTestBed CreateWeb<TTest>(TestContext context, string webAppPath)
        {
            return new WebTestBed(typeof(TTest), context, webAppPath);
        }

        public static DesktopTestBed CreateDesktop<TTest>(TestContext context)
        {
            return new DesktopTestBed(typeof(TTest), context);
        }

        public static TestBed Create<TTest>(TestContext context)
        {
            return new TestBed(typeof(TTest), context);
        }

        public void AssertEqual(string actual, string fileName)
        {
            AssertEqual(Encoding.UTF8.GetBytes(actual), fileName);
        }

        public void AssertEqual(byte[] actual, string fileName)
        {
            AssertEqual(new MemoryStream(actual), fileName);
        }

        public void AssertEqual(Stream actual, string fileName)
        {
            var actualBytes = actual.ReadAllBytes();
            File.WriteAllBytes(Path.Combine(actualResultBasePath, fileName), actualBytes);

            var expectedResourceKey = testType.Namespace + ".Expected." + fileName;
            var expectedStream = testType.Assembly.GetManifestResourceStream(expectedResourceKey);

            if (expectedStream == null)
            {
                Assert.Fail("Could not find the expected result for {0}", fileName);
                return;
            }

            if (fileName.EndsWith(".png"))
            {
                FileEqualityComparer.AssertEqualImages(expectedStream.ReadAllBytes(), actualBytes);
            }
            else if (fileName.EndsWith(".svg") || fileName.EndsWith(".html"))
            {
                FileEqualityComparer.AssertEqualText(expectedStream.ReadAllBytes(), actualBytes);
            }
            else
            {
                FileEqualityComparer.AssertEqualBinary(expectedStream.ReadAllBytes(), actualBytes);
            }
        }

        public virtual void Dispose()
        {
        }

        protected static string GetSolutionDir()
        {
            var directory = new FileInfo(typeof(TestBed).Assembly.Location).Directory;

            while (directory != null)
            {
                if (directory.EnumerateFiles("*.sln").Any())
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            throw new DirectoryNotFoundException("Could not find solution directory.");
        }
    }
}
