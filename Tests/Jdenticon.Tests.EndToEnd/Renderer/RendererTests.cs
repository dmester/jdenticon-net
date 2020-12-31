using Jdenticon.Drawing;
using Jdenticon.Rendering;
using Jdenticon.Tests.EndToEnd.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MemoryStream = System.IO.MemoryStream;

namespace Jdenticon.Tests.EndToEnd.Renderer
{
    [TestClass]
    public class RendererTests
    {
        public TestContext TestContext { get; set; }

        /**
         * This test generates a png out of a set of polygons that
         * are constructed to cover as many possibilities as possible.
         * 
         * The same test is performed in the JS and PHP versions.
         */

        [TestMethod]
        public void Test()
        {
            using (var testBed = TestBed.Create<RendererTests>(TestContext))
            {
                var bitmap = new Bitmap(100, 100);

                // Diamond background, should be clipped to viewport
                bitmap.FillPolygon(Color.FromArgb(30, 0, 0, 255), new PointF[]
                {
                    new PointF(10,10),
                    new PointF(10,90),
                    new PointF(90,90),
                    new PointF(90,10),
                    new PointF(10,10)
                });

                // Entirely contained rectangle
                bitmap.FillPolygon(Color.FromArgb(230, 40, 40, 40), new PointF[]
                {
                    new PointF(50,-15),
                    new PointF(115,50),
                    new PointF(50,115),
                    new PointF(-15,50)
                });

                // Center figures with holes
                bitmap.FillPath(Color.Yellow, GetDiamond(20, 30, 10));
                bitmap.FillPath(Color.FromArgb(128, 0, 0, 255), GetDiamond(40, 30, 10));

                // The following shapes should not be rendered.
                bitmap.FillPolygon(Color.Red, new PointF[]
                {
                    new PointF(-10,-15),
                    new PointF(-5,-10),
                    new PointF(0,110),
                    new PointF(-15,115)
                });

                bitmap.FillPolygon(Color.Red, new PointF[]
                {
                    new PointF(115,-15),
                    new PointF(120,-10),
                    new PointF(120,110),
                    new PointF(115,115)
                });

                var surrounding = new Drawing.Path();
                surrounding.AddPolygon(new PointF[]
                {
                    new PointF(-20,-20),
                    new PointF(-20,120),
                    new PointF(120,120),
                    new PointF(120,-20)
                });

                // Hole
                surrounding.AddPolygon(new PointF[]
                {
                    new PointF(-10,-10),
                    new PointF(110,-10),
                    new PointF(110,110),
                    new PointF(-10,110),
                });

                // Black with antialiasing.
                // Used to ensure there are no rounding errors.
                bitmap.FillPolygon(Color.Black, new PointF[]{
                    new PointF(5,5),
                    new PointF(15,5),
                    new PointF(25,25),
                    new PointF(15,25)
                });
                bitmap.FillPolygon(Color.Black, new PointF[]{
                    new PointF(15,5),
                    new PointF(25,5),
                    new PointF(15,25),
                    new PointF(5,25)
                });

                // White with antialiasing.
                // Used to ensure there are no rounding errors.
                bitmap.FillPolygon(Color.White, new PointF[]{
                    new PointF(5,75),
                    new PointF(15,75),
                    new PointF(25,95),
                    new PointF(15,95)
                });
                bitmap.FillPolygon(Color.White, new PointF[]{
                    new PointF(15,75),
                    new PointF(25,75),
                    new PointF(15,95),
                    new PointF(5,95)
                });

                using (var stream = new MemoryStream())
                {
                    bitmap.SaveAsPng(stream);
                    stream.Position = 0;
                    testBed.AssertEqual(stream, "render.png");
                }
            }
        }

        private static Path GetDiamond(int x, int y, int cellsize)
        {
            var path = new Path();

            path.AddPolygon(new PointF[]
            {
                new PointF(x, y + cellsize * 2),
                new PointF(x + cellsize * 2, y),
                new PointF(x + cellsize * 4, y + cellsize * 2),
                new PointF(x + cellsize * 2, y + cellsize * 4)
            });

            path.AddPolygon(new PointF[]
            {
                new PointF(x + cellsize * 1, y + cellsize * 2),
                new PointF(x + cellsize * 0, y + cellsize * 4),
                new PointF(x + cellsize * 2, y + cellsize * 3),
                new PointF(x + cellsize * 3, y + cellsize * 2),
                new PointF(x + cellsize * 2, y + cellsize * 1)
            });

            return path;
        }
    }
}
