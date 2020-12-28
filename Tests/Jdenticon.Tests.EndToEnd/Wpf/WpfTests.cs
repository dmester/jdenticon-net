using Jdenticon.Tests.EndToEnd.Helpers;
using Jdenticon.Wpf.Sample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace Jdenticon.Tests.EndToEnd.Wpf
{
    [TestClass]
    public class WpfTests
    {
        private static DesktopTestBed testBed;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBed = TestBed.CreateDesktop<WpfTests>(context);
        }

        [TestMethod]
        public void Test()
        {
            var window = new MainWindow();
            try
            {
                window.Show();

                testBed.AssertEqualWindow(window, "window.png");
            }
            finally
            {
                window.Close();
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testBed?.Dispose();
        }
    }
}
