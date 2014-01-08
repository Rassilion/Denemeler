using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ForgeBot
{
    internal class CachedBitmapReader:IDisposable
    {
        private bool _disposed = false; // to detect redundant calls

        private IntPtr _imageSrcPtr = IntPtr.Zero;
        private IntPtr _bitmapContext = IntPtr.Zero;
        private IntPtr _bitmapBuffer = IntPtr.Zero;
        private IntPtr _sourceImageBuffer = IntPtr.Zero;
        private GdiInterop.BITMAPINFO _bih = null;

        private Bitmap _cachedBitmap=null;

        private int _height;
        private int _width;

        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }
        internal IntPtr GetSourceImageHBitmap
        {
            get { return _imageSrcPtr; }
        }
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Creates or retreives data part of a bitmap buffer to be used for drawing
        /// </summary>
        /// <param name="width">width of new cached bitmap to be created</param>
        /// <param name="height">height of new cached bitmap to be created</param>
        /// <param name="destinationImageBuffer">outputs IntPtr data buffer that can be used to access pixels</param>
        internal void GetCachedBitmapBuffer(int width, int height, out IntPtr destinationImageBuffer)
        {
            if(_disposed)
                throw new ObjectDisposedException("CachedBitmapReader");
            if (_bitmapContext == IntPtr.Zero)
                _bitmapContext = GdiInterop.CreateCompatibleDC(IntPtr.Zero);
            if (_bih != null && (width != _bih.biWidth || height != Math.Abs(_bih.biHeight)))
            {
                _bih = null;
                GdiInterop.DeleteObject(_imageSrcPtr);
                _imageSrcPtr = IntPtr.Zero;
            }
            if (_imageSrcPtr == IntPtr.Zero)
            {
                _width = width;
                _height = height; 
                
                _bih = new GdiInterop.BITMAPINFO();
                _bih.biSize = 40;
                _bih.biBitCount = 32;
                _bih.biCompression = (int)GdiInterop.BICompressionType.BI_RGB;
                _bih.biPlanes = 1;
                _bih.biWidth = width;
                _bih.biHeight = -height; // fixes problem with vertical flip (the way image is copied)

                // creates HBitmap object
                _imageSrcPtr = GdiInterop.CreateDIBSection(_bitmapContext, _bih,
                                                           (int) GdiInterop.DIBColorsType.DIB_RGB_COLORS,
                                                           out _bitmapBuffer, IntPtr.Zero, 0);

            }

            destinationImageBuffer = _bitmapBuffer;
        }

        internal Bitmap GetCachedBitmap(int width, int height)
        {
            if (_disposed)
                throw new ObjectDisposedException("CachedBitmapReader");

            if (_cachedBitmap != null && (width != _cachedBitmap.Width || height != Math.Abs(_cachedBitmap.Height)))
            {
                _cachedBitmap.Dispose();
                _cachedBitmap = null;
            }
            if (_cachedBitmap == null)
            {
                _width = width;
                _height = height;

                _cachedBitmap = new Bitmap(width, height);

            }

            return _cachedBitmap;
        }

       
        #region IDisposable implementation
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
                    // Dispose managed resources.
                    ReleaseManagedObjects();
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
                ReleaseUnmanagedObjects();
            }
            _disposed = true;

        }

        private void ReleaseManagedObjects()
        {
            if(_cachedBitmap!=null)
                _cachedBitmap.Dispose();
        }

        void ReleaseUnmanagedObjects()
        {
            if (_imageSrcPtr != IntPtr.Zero)
            {
                GdiInterop.DeleteObject(_imageSrcPtr);
                _imageSrcPtr = IntPtr.Zero;
            }
            if (_bitmapContext != IntPtr.Zero)
            {
                GdiInterop.DeleteDC(_bitmapContext);
                _bitmapContext = IntPtr.Zero;
            }
        }

        ~CachedBitmapReader()
        {
            Dispose(false);
        }
        #endregion


    }
}
