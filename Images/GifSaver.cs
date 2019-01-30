using System.Drawing;

namespace Images
{
    public class GifSaver
    {
        private readonly Bitmap[] _bitmaps;

        public GifSaver(params Bitmap[] bitmaps)
        {
            _bitmaps = bitmaps;
        }

        public void SaveToFile(string path)
        {
            using (var gif = AnimatedGif.AnimatedGif.Create(path, 1))
            {
                foreach (var image in _bitmaps)
                {
                    gif.AddFrame(image);
                }
            }
        }
    }
}
