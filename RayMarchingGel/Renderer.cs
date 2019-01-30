using System;
using System.Drawing;
using System.Threading.Tasks;
using Images;
using Vectors;

namespace RayMarchingGel
{
    public class Renderer
    {
        #region PRIVATEVARS

        private Vector3 _primitivesColorVector3;

        private const double TimeDivider = 5.0;

        #endregion

        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int FramesCount { get; set; }
        public int RenderDepth { get; set; }
        public Color BackroundColor { get; set; }
        public double GridPositionZ { get; set; }
        public Vector3 CameraPosition { get; set; }
        public Vector3 LightPosition { get; set; }
        public Primitive ObjectToRender { get; set; }

        public Color PrimitivesColor
        {
            get { return _primitivesColorVector3.VectorToColor(); }
            set { _primitivesColorVector3 = new Vector3(value.R, value.G, value.B); }
        }

        public Renderer(int imageHeight, int imageWidth, int framesCount)
        {
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
            FramesCount = framesCount;

            BackroundColor = Color.DarkGray;
            PrimitivesColor = Color.White;
            RenderDepth = 20;
            GridPositionZ = -7;
            CameraPosition = new Vector3(0, 0, -20);
            LightPosition = new Vector3(5, 5, -10);
        }

        public Bitmap[] Render()
        {
            if(ObjectToRender == null)
                throw new ArgumentNullException(nameof(ObjectToRender));

            Bitmap[] bitmaps = new Bitmap[FramesCount];

            Parallel.For(0, FramesCount, t =>
            {
                var time = t / TimeDivider;
                bitmaps[t] = new Bitmap(ImageWidth, ImageHeight);

                using (BitmapSimpleWrapper bw = new BitmapSimpleWrapper(bitmaps[t]))
                {
                    RenderOneFrame(bw, time);
                }
            });

            return bitmaps;
        }

        private void RenderOneFrame(BitmapSimpleWrapper bw, double time)
        {
            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
                    Vector3 currentCell = new Vector3((i - ImageWidth / 2) / 100.0, -(j - ImageHeight / 2) / 100.0, GridPositionZ);
                    Ray ray = new Ray(currentCell, (currentCell - CameraPosition).Normalized);

                    Vector3 marchingResult = MarchingRay(ray, RenderDepth, time);

                    if (marchingResult != null)
                    {
                        Vector3 pixelColor = _primitivesColorVector3;
                        var lightIntensity = Math.Min(ComputePhongReflection(marchingResult, GetNormal(marchingResult, time)), 1);

                        pixelColor *= lightIntensity;
                        bw.SetPixel(i, j, pixelColor.VectorToColor());
                        continue;
                    }
                    bw.SetPixel(i, j, BackroundColor);
                }
            }
        }

        private Vector3 GetNormal(Vector3 pos, double time)
        {
            const double eps = 0.1;
            var d = SignedDistance(pos, time);
            var nx = SignedDistance(pos + new Vector3(eps, 0, 0), time) - d;
            var ny = SignedDistance(pos + new Vector3(0, eps, 0), time) - d;
            var nz = SignedDistance(pos + new Vector3(0, 0, eps), time) - d;

            return new Vector3(nx, ny, nz).Normalized;
        }

        private double SignedDistance(Vector3 point, double time)
        {
            return ObjectToRender.SignedDistance(point, time);
        }

        private Vector3 MarchingRay(Ray ray, int depth, double time)
        {
            var prevSf = Double.MaxValue;
            for (var k = 0; k < depth; k++)
            {
                var sf = SignedDistance(ray.StartPoint, time);

                if (sf <= 0)
                    return ray.StartPoint;

                if (prevSf < sf) //hack for speed-up, but its not correct in other cases
                    return null;

                ray.StartPoint = ray.StartPoint + ray.Direction * Math.Max(sf * 0.1, 0.01);
                prevSf = sf;
            }
            return null;
        }

        private double ComputePhongReflection(Vector3 point, Vector3 normalToSurface)
        {
            var ambient = 0.1;
            var diffuse = 0.8;
            var specular = 2;
            var shininess = 100;

            double illumination = ambient;

            var vectorToLight = (LightPosition - point).Normalized;
            var dotNormAndLight = Vector3.Dot(vectorToLight, normalToSurface);
            var lightReflection = (2 * dotNormAndLight * normalToSurface - vectorToLight).Normalized;

            illumination += diffuse * dotNormAndLight +
                            specular *
                            Math.Pow(Vector3.Dot(lightReflection, (CameraPosition - point).Normalized),
                                shininess);

            return illumination > 0 ? illumination : 0;
        }
    }
}
