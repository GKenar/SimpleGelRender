using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Images;
using Vectors;

namespace RayMarchingGel
{
    class Program
    {
        private static void Main()
        {
            var primitives = new List<Primitive>
            {
                new CrumpledSphere(1.7, new Vector3(-1.5, 0, 0), 0.2),
                new CrumpledSphere(1.7, new Vector3(1.5, 0, 0), 0.2)
            };
            SmoothContainer sc = new SmoothContainer(primitives, 5);

            var renderer = new Renderer(400, 400, 15)
            {
                ObjectToRender = sc,
                PrimitivesColor = Color.YellowGreen
            };

            var renderRusult = renderer.Render();

            GifSaver saver = new GifSaver(renderRusult);
            saver.SaveToFile("test.gif");

            Process.Start("test.gif");
        }
    }
}
