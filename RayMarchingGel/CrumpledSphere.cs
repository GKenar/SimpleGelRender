using System;
using Vectors;

namespace RayMarchingGel
{
    public class CrumpledSphere : Primitive
    {
        private readonly static Random RandomGenerator;
        private readonly double _rndTimeShift;
        public double Radius { get; set; }
        public double CrumpledCoefficient { get; set; }

        public CrumpledSphere(double radius, Vector3 position, double crumpledCoefficient)
        {
            Radius = radius;
            Position = position;
            CrumpledCoefficient = crumpledCoefficient;

            _rndTimeShift = RandomGenerator.NextDouble() * 2.0 * Math.PI;
        }

        static CrumpledSphere()
        {
            RandomGenerator = new Random();
        }

        public override double SignedDistance(Vector3 point, double time)
        {
            var centered = Position - point;
            time += _rndTimeShift;
            return centered[0] * centered[0] + centered[1] * centered[1] + centered[2] * centered[2] - Radius * Radius 
                - (Math.Sin(centered[0] * 3 + time) * Math.Sin(centered[1] * 3 + time) * Math.Sin(centered[2] * 3 + time) + 1) * CrumpledCoefficient;
        }
    }
}
