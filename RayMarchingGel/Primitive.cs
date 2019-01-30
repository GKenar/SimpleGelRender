using Vectors;

namespace RayMarchingGel
{
    public abstract class Primitive
    {
        public Vector3 Position { get; set; }

        public abstract double SignedDistance(Vector3 point, double time);
    }
}
