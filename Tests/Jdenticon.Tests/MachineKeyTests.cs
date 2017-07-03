/*
using Jdenticon.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Tests
{
    [TestClass]
    public class MachineKeyTests
    {
        [TestMethod]
        public void MachineKey_Protect()
        {
            var data = new byte[14];
            var random = new Random();
            random.NextBytes(data);

            var encrypted = MachineKeyEx.Protect(data);
            
            var decrypted = MachineKeyEx.Unprotect(encrypted);
            
            Assert.AreEqual(data.Length, decrypted.Length);

            for (var i=0;i<data.Length;i++)
            {
                Assert.AreEqual(data[i], decrypted[i]);
            }
        }
    }
}
*/