using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Images
{
    public class BitmapSimpleWrapper : IDisposable
    {
        private readonly Bitmap _bitmap;

        private byte[] _rgbBytes;

        private BitmapData _bmpData;

        public int Width => _bitmap.Width;

        public int Height => _bitmap.Height;

        public BitmapSimpleWrapper(Bitmap bitmap)
        {
            _bitmap = bitmap;

            LockBitmapAndGetData();
        }

        public void SetPixel(int x, int y, Color c)
        {
            int shift = _bmpData.Stride * y + 4 * x;

            if (shift < 0 || shift >= _rgbBytes.Length)
                return;

            _rgbBytes[shift] = c.B;
            _rgbBytes[shift + 1] = c.G;
            _rgbBytes[shift + 2] = c.R;
        }

        private void LockBitmapAndGetData()
        {
            var rect = new Rectangle(0, 0, _bitmap.Width, _bitmap.Height);
            _bmpData = _bitmap.LockBits(rect, ImageLockMode.ReadWrite, _bitmap.PixelFormat);
            _rgbBytes = new byte[Math.Abs(_bmpData.Stride) * _bmpData.Height];

            Marshal.Copy(_bmpData.Scan0, _rgbBytes, 0, _rgbBytes.Length);
        }

        private void UnlockBitmapAndSaveResult()
        {
            Marshal.Copy(_rgbBytes, 0, _bmpData.Scan0, _rgbBytes.Length);
            _bitmap.UnlockBits(_bmpData);
        }

        public void Dispose()
        {
            UnlockBitmapAndSaveResult();
        }
    }
}
