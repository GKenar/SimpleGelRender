using System.Drawing;

namespace Vectors
{
    public static class Vector3Helper
    {
        public static Color VectorToColor(this Vector3 vector)
        {
            return Color.FromArgb((int)vector.X, (int)vector.Y, (int)vector.Z);
        }
    }
}
