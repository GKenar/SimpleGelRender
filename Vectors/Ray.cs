namespace Vectors
{
    public struct Ray
    {
        public Vector3 StartPoint;
        public Vector3 Direction;

        public Ray(Vector3 startPoint, Vector3 direction)
        {
            StartPoint = startPoint;
            Direction = direction;
        }

        public Ray(Vector3 direction) : this(new Vector3(), direction)
        { }


    }
}
