using Jdenticon.Tests.EndToEnd.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.AspNetMvc
{
    [TestClass]
    public class AspNetMvcTests
    {
        private static WebTestBed testBed;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBed = TestBed.CreateWeb<AspNetMvcTests>(context, "Samples\\Jdenticon.AspNet.Mvc.Sample");
        }

        [DataTestMethod]
        [DataRow("/", "index.html")]
        [DataRow("/mvc/icon/Test/100", "mvc.png")]
        [DataRow("/api/icon/Test/100", "webapi.png")]
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
