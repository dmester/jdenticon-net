using Jdenticon.Tests.EndToEnd.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.AspNetWebForms
{
    [TestClass]
    public class AspNetWebFormsTests
    {
        private static WebTestBed testBed;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBed = TestBed.CreateWeb<AspNetWebFormsTests>(context, "Samples\\Jdenticon.AspNet.WebForms.Sample");
        }

        [TestMethod]
        [DataRow("/", "index.html")]
        [DataRow("/identicon.axd?MhsAZAqyuuB7f7XR~g--", "axd.png")]
        public async Task TestUrl(string url, string fileName)
        {
            await testBed.AssertEqualHttpAsync(url, fileName);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testBed?.Dispose();
        }
    }
}
