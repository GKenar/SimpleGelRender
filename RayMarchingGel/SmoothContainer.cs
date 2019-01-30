using System;
using System.Collections.Generic;
using System.Linq;
using Vectors;

namespace RayMarchingGel
{
    class SmoothContainer : Primitive
    {
        public List<Primitive> Primitives { get; set; }

        public double SmoothCoefficient { get; set; }

        public SmoothContainer(List<Primitive> primitives, double smoothCoefficient)
        {
            Primitives = primitives;
            SmoothCoefficient = smoothCoefficient;
        }

        public override double SignedDistance(Vector3 point, double time)
        {
            double d = Double.MaxValue;

            if (!Primitives.Any())
                return d;

            d = Primitives[0].SignedDistance(point, time);

            for (var i = 1; i < Primitives.Count(); i++)
            {
                d = Math.Min(d, PrimitivesOperations.SmoothUnion(d, Primitives[i].SignedDistance(point, time), SmoothCoefficient));
            }

            return d;
        }
    }
}
