using Jdenticon.Tests.EndToEnd.Helpers;
using Jdenticon.WinForms.Sample;
using Jdenticon.Wpf.Sample;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jdenticon.Tests.EndToEnd.WinForms
{
    [TestClass]
    public class WinFormsTests
    {
        private static DesktopTestBed testBed;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBed = TestBed.CreateDesktop<WinFormsTests>(context);
        }

        [TestMethod]
        public void Test()
        {
            using (var form = new MainForm())
            {
                try
                {
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                    
                    testBed.AssertEqualWindow(form, "window.png");
                }
                finally
                {
                    //form.Close();
                }
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testBed?.Dispose();
        }
    }
}
