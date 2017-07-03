using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.Cryptography;

namespace Jdenticon.Tests
{
    [TestClass]
    public class HashUtilsTests
    {
        [TestMethod]
        public void HashUtils_Numeric()
        {
            Assert.AreEqual("827ccb0eea8a706c4c34a16891f84e7b", HexString.ToString(HashGenerator.ComputeHash(12345, "MD5")));
        }

        [TestMethod]
        public void HashUtils_String()
        {
            Assert.AreEqual("827ccb0eea8a706c4c34a16891f84e7b", HexString.ToString(HashGenerator.ComputeHash("12345", "MD5")));
        }

        [TestMethod]
        public void HashUtils_EmptyString()
        {
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", HexString.ToString(HashGenerator.ComputeHash("", "MD5")));
        }

        [TestMethod]
        public void HashUtils_Null()
        {
            Assert.AreEqual("d41d8cd98f00b204e9800998ecf8427e", HexString.ToString(HashGenerator.ComputeHash(null, "MD5")));
        }
    }
}
