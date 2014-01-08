using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ForgeBot
{
    internal unsafe class Screenshot:IDisposable
    {
        private bool _disposed = false; // to detect redundant calls

        private byte* _rgbValues;
        private IntPtr _pictureBuffer;
        private int _height;
        private int _width;
        private int _bytesPerPixel;
        private int _bufferLength;
        private Bitmap _originalImage;

        public Screenshot(IntPtr pictureBuffer,int pictureWidth,int pictureHeight,int bytesPerPixel)
        {
            _pictureBuffer = pictureBuffer;
            _rgbValues = (byte*) pictureBuffer.ToPointer();
            _height = pictureHeight;
            _width = pictureWidth;
            _bytesPerPixel = bytesPerPixel;
            _bufferLength = pictureHeight*pictureWidth*bytesPerPixel;
        }

        public int GetPixelRgb(int x, int y)
        {
            if(_disposed)
                throw new ObjectDisposedException("Screenshot");
            if (_rgbValues == (byte*)0)
                return 0;
            int position = (y * _width) * _bytesPerPixel + x * _bytesPerPixel;
            if (position + 3 >= _bufferLength || position < 0)
                return 0;

            return (int)(((ulong)((((_rgbValues[position + 2] << 16) | (_rgbValues[position + 1] << 8)) | _rgbValues[position]) | (0xFF << 0x18))) & 0xffffffffL);

            //return Color.FromArgb(_rgbValues[position + 3],_rgbValues[position + 2], _rgbValues[position + 1], _rgbValues[position]).ToArgb();
        }

        public Bitmap Image
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException("Screenshot"); 
                if (_originalImage == null && _rgbValues != null)
                {
                    _originalImage=new Bitmap(_width,_height,PixelFormat.Format32bppRgb);
                    var bitmapData = _originalImage.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.WriteOnly,
                                                             _originalImage.PixelFormat);

                    Win32.memcpy(bitmapData.Scan0, _pictureBuffer,
                                new UIntPtr((uint)(bitmapData.Stride * bitmapData.Height)));

                    _originalImage.UnlockBits(bitmapData);

                }
                return _originalImage;
            }
        }

        #region IDisposable implementation

        void ReleaseUnmanagedObjects()
        {

        }

        void ReleaseManagedObjects()
        {
            if (_originalImage != null)
                _originalImage.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // here we release managed objects
                    ReleaseManagedObjects();
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
                ReleaseUnmanagedObjects();
            }
            _disposed = true;

        }

       
        ~Screenshot()
        {
            Dispose(false);
        }
        #endregion
    }
}
