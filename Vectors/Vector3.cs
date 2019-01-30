using System;

namespace Vectors
{
    public class Vector3
    {
        public double this[int coord]
        {
            get { return _coords[coord]; }
            set { _coords[coord] = value; }
        }

        public double Length => Math.Sqrt(_coords[0] * _coords[0] + _coords[1] * _coords[1] + _coords[2] * _coords[2]);

        public Vector3 Normalized
        {
            get
            {
                var len = Length;
                return new Vector3(_coords[0] / len, _coords[1] / len, _coords[2] / len);
            }
        }

        public double X => _coords[0];
        public double Y => _coords[1];
        public double Z => _coords[2];

        private readonly double[] _coords;

        public Vector3(double x = 0, double y = 0, double z = 0)
        {
            _coords = new double[3];

            _coords[0] = x;
            _coords[1] = y;
            _coords[2] = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._coords[0] + v2._coords[0],
                               v1._coords[1] + v2._coords[1],
                               v1._coords[2] + v2._coords[2]);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1._coords[0] - v2._coords[0],
                               v1._coords[1] - v2._coords[1],
                               v1._coords[2] - v2._coords[2]);
        }

        public static Vector3 operator *(double x, Vector3 v)
        {
            return new Vector3(x * v._coords[0],
                               x * v._coords[1],
                               x * v._coords[2]);
        }

        public static Vector3 operator *(Vector3 v, double x)
        {
            return x * v;
        }

        public static double Dot(Vector3 v1, Vector3 v2)
        {
            return v1._coords[0] * v2._coords[0] +
                   v1._coords[1] * v2._coords[1] +
                   v1._coords[2] * v2._coords[2];
        }

        public override string ToString()
        {
            return string.Format("({0:0.###}, {1:0.###}, {2:0.###})", _coords[0], _coords[1], _coords[2]);
        }
    }
}
