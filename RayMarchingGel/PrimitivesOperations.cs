using System;

namespace RayMarchingGel
{
    public static class PrimitivesOperations
    {
        public static double SmoothUnion(double d1, double d2, double k)
        {
            var h = Math.Max(Math.Min(0.5 + 0.5 * (d2 - d1) / k, 1), 0);
            return Lerp(d2, d1, h) - k * h * (1.0 - h);
        }

        private static double Lerp(double a, double b, double l)
        {
            return a + (b - a) * l;
        }
    }
}
