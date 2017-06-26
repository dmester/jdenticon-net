using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Linq;

namespace Jdenticon.Tests
{
    [TestClass]
    public class MD5Tests
    {
        private static byte[] GenerateMessage(int size)
        {
            var random = new Random(0);
            var bytes = new byte[size];
            random.NextBytes(bytes);
            return bytes;
        }

        [TestMethod]
        public void MD5_ComputeHash()
        {
            // Compare result with .NET implementation
            var sizesToTest = new[] { 0, 1, 2, 3, 4, 60, 61, 62, 63, 64, 65, 66, 127, 128, 129, 45123, 1 << 20 };
            var md5 = MD5.Create();

            foreach (var sizeToTest in sizesToTest)
            {
                var message = GenerateMessage(sizeToTest);
                var hash1 = md5.ComputeHash(message);
                var hash2 = Jdenticon.Cryptography.MD5.ComputeHash(message);

                Assert.IsTrue(hash1.SequenceEqual(hash2));
            }
        }
    }
}
