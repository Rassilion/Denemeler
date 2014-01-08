using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Awesomium.Core;

namespace ForgeBot
{
    public class FastBitmapSurfaceUpdatedEventArgs : EventArgs
    {
        private AweRect _dirtyRegion;
        internal FastBitmapSurfaceUpdatedEventArgs(AweRect dirtyRegion)
        {
            _dirtyRegion = dirtyRegion;
        }

        public AweRect DirtyRegion
        {
            get { return _dirtyRegion; }
        }
    }

    public class FastBitmapSurfaceInitializedEventArgs : EventArgs
    {
        internal FastBitmapSurfaceInitializedEventArgs()
        {
        }
    }

    public class FastBitmapSurface:ISurface,IDisposable
    {
#if DEBUG
        public delegate void LogWriter(string logMessage);
        private LogWriter Logger;
#endif
        private bool _disposed = false; // to detect redundant calls

        #region event handlers and delegates
        public EventHandler<FastBitmapSurfaceUpdatedEventArgs> Updated;
        public EventHandler<FastBitmapSurfaceInitializedEventArgs> Initialized;
        #endregion

        private IntPtr _internalBuffer=IntPtr.Zero;
        private IntPtr _tempScrollingBuffer=IntPtr.Zero;
        private int _width;
        private int _height;
        private const int BytesPerPixel = 4;
        private IWebView _webView;
        private bool _isDirty = false;
        private AweRect _dirtyRegion = AweRect.Empty;

        public IWebView View
        {
            get { return _webView; }
        }

        public bool IsDisposed
        {
            get { return _disposed; }
        }

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public int Width
        {
            get { return _width; }
        }

        public AweRect DirtyRegion
        {
            get { return _dirtyRegion; }
            protected set { _dirtyRegion = value; }
        }

        public int Height
        {
            get { return _height; }
        }

        public IntPtr Buffer
        {
            get { return _internalBuffer; }
        }

#if DEBUG
        public FastBitmapSurface(int width, int height,LogWriter logger)
        {
            Logger = logger;
#else
        public FastBitmapSurface(int width, int height)
        {
#endif
            if (width != _width || height != _height || _internalBuffer == IntPtr.Zero)
                ReloadMemoryChunk(width, height);

            _width = width;
            _height = height;

            ClearDirty();

        }

        public void ClearDirty()
        {
            _isDirty = false;
            _dirtyRegion=AweRect.Empty;
        }

        public void Initialize(IWebView view, int width, int height)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");
            if (width != _width || height != _height || _internalBuffer == IntPtr.Zero)
                ReloadMemoryChunk(width, height);

            _width = width;
            _height = height;
            _webView = view;
            ClearDirty();

            OnInitialized();
        }
        
        /// <summary>
        /// This method is called whenever the IWebView instance this surface is assigned to, wants to paint a certain section of the Surface with a block of pixels. It is your responsibility to copy srcBuffer to the location in this Surface specified by destRect.
        /// </summary>
        /// <param name="srcBuffer">A pointer to a block of pixels in 32-bit BGRA format. The size of the buffer is: srcRowSpan * srcRect.Height.</param>
        /// <param name="srcRowSpan">The number of bytes of each row. (Usually: srcRect.Width * 4)</param>
        /// <param name="srcRect">The dimensions of the region of srcBuffer to copy from. May have a non-zero origin.</param>
        /// <param name="destRect">The location to copy srcBuffer to. Always has same dimensions as srcRect but may have different origin (which specifies the offset of the section to copy to).</param>
        public void Paint(IntPtr srcBuffer, int srcRowSpan, AweRect srcRect, AweRect destRect)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");

            CopyMemoryRectangular(srcBuffer, _internalBuffer, srcRowSpan, _width*BytesPerPixel, srcRect, destRect,
                                  BytesPerPixel);

            OnUpdated(CalculateTotalUpdatedRectangle(destRect));
        }

        private AweRect CalculateTotalUpdatedRectangle(AweRect destRect)
        {
            _isDirty = true;
            if (_dirtyRegion == AweRect.Empty)
            {
                _dirtyRegion = destRect;
                return _dirtyRegion;
            }
            else
                _dirtyRegion=new AweRect(_width,_height,0,0);

            _dirtyRegion=new AweRect(destRect.X<_dirtyRegion.X?destRect.X:_dirtyRegion.X,
                destRect.Y<_dirtyRegion.Y?destRect.Y:_dirtyRegion.Y,
                destRect.Width>_dirtyRegion.Width?destRect.Width:_dirtyRegion.Width,
                destRect.Height>_dirtyRegion.Height?destRect.Height:_dirtyRegion.Height
                );
            return _dirtyRegion;
        }

        public void Scroll(int dx, int dy, AweRect clipRect)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");
            int srcRowSpan = _width * BytesPerPixel;
            int destRowSpan = srcRowSpan;

            int srcHeight = clipRect.Height - Math.Abs(dy);
            int srcWidth = clipRect.Width - Math.Abs(dx);

            int srcX=0, srcY=0,dstX=0,dstY=0;
            if (dy != 0)
            {
                if (dy < 0)
                    srcY = -dy;
                else
                    dstY = dy;
            }
            if (dx != 0)
            {
                if (dx < 0)
                    srcX = -dx;
                else
                    dstX = dx;
            }
            var srcRect = new AweRect(srcX, srcY, srcWidth, srcHeight);
            var destRect = new AweRect(dstX, dstY, srcWidth, srcHeight);

            CopyMemoryRectangular(_internalBuffer, _tempScrollingBuffer, srcRowSpan, destRowSpan, srcRect, srcRect, 
                                  BytesPerPixel);
            CopyMemoryRectangular(_tempScrollingBuffer, _internalBuffer, srcRowSpan, destRowSpan, srcRect, destRect,
                                  BytesPerPixel);

            OnUpdated(CalculateTotalUpdatedRectangle(clipRect));
        }

        public void SaveToPNG(string filename)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");
            SaveToFile(filename, _internalBuffer, ImageFormat.Png);
        }

        public void SaveToJPG(string filename)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");
            SaveToFile(filename, _internalBuffer, ImageFormat.Jpeg);
        }

        /// <summary>
        /// Saves image from buffer to file
        /// </summary>
        /// <param name="filename">file name</param>
        /// <param name="bufferToUse">IntPtr of a buffer holding image</param>
        /// <param name="format">file format</param>
        private void SaveToFile(string filename, IntPtr bufferToUse, ImageFormat format)
        {
            using (var _originalImage = new Bitmap(_width, _height))
            {
                var bitmapData = _originalImage.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.WriteOnly,
                                                         PixelFormat.Format32bppRgb);

                Win32.memcpy(bitmapData.Scan0, bufferToUse,
                            new UIntPtr((uint)(bitmapData.Stride*bitmapData.Height)));

                _originalImage.UnlockBits(bitmapData);
                _originalImage.Save(filename,format);
            }
        }

        /// <summary>
        /// Called when an instance of this class is being disposed.
        /// </summary>
        protected virtual void OnDispose()
        {

        }
        
        /// <summary>
        /// Copies this bitmap to a certain destination. This will also set IsDirty to false. 
        /// </summary>
        /// <param name="destBuffer">A pointer to the destination pixel buffer. </param>
        /// <param name="destRowSpan">The number of bytes per-row of the destination. </param>
        /// <param name="destDepth">The depth (number of bytes per pixel; usually 4 for BGRA surfaces and 3 for BGR surfaces). </param>
        public void CopyTo(IntPtr destBuffer,int destRowSpan,int destDepth)
        {
            if (_disposed)
                throw new ObjectDisposedException("FastBitmapSurface");
            Win32.memcpy(destBuffer, _internalBuffer,
                            new UIntPtr((uint)(destRowSpan*_height)));
            ClearDirty();
        }

        private void ReloadMemoryChunk(int width, int height)
        {
            ReleaseUnmanagedObjects();
            _internalBuffer = Marshal.AllocHGlobal(width * BytesPerPixel * height);
            _tempScrollingBuffer = Marshal.AllocHGlobal(width * BytesPerPixel * height);

        }

        private void OnInitialized()
        {
            if (Initialized != null)
                Initialized(this, new FastBitmapSurfaceInitializedEventArgs());
        }

        private void OnUpdated(AweRect clipRect)
        {
            if (Updated != null)
            {
                Updated(this, new FastBitmapSurfaceUpdatedEventArgs(clipRect));
            }
        }

        /// <summary>
        /// Copy rectangle chunk from source to destination rectangle chunk of the same size
        /// </summary>
        /// <param name="srcBuffer">A pointer to a block of pixels in 32-bit BGRA format. The size of the buffer is: srcRowSpan * srcRect.Height.</param>
        /// <param name="destBuffer">A pointer to a block of pixels in 32-bit BGRA format. The size of the buffer is: srcRowSpan * srcRect.Height.</param>
        /// <param name="srcRowspan">The number of bytes per-row of the source - of small image (only the region that needs to be updated)</param>
        /// <param name="dstRowspan">The number of bytes per-row of the destination</param>
        /// <param name="srcRect">The dimensions of the region of srcBuffer to copy from. May have a non-zero origin.</param>
        /// <param name="destRect">The location to copy srcBuffer to. Always has same dimensions as srcRect but may have different origin (which specifies the offset of the section to copy to).</param>
        internal static void CopyMemoryRectangular(IntPtr srcBuffer, IntPtr destBuffer, int srcRowspan,int dstRowspan, AweRect srcRect, AweRect destRect,int bytesPerPixel)
        {
            int startSrcX = srcRect.X*BytesPerPixel;
            int startSrcY = srcRect.Y;
            int startDstX = destRect.X*BytesPerPixel;
            int startDstY = destRect.Y;
            int rowDst = startDstY;

            int copyWidth = srcRect.Width*bytesPerPixel;

            IntPtr fromPtr, toPtr;
            for (int rowSrc = startSrcY; rowSrc < srcRect.Height+startSrcY; rowSrc++)
                //Parallel.For(startSrcY, srcRect.Height , (rowSrc) =>
            {
                fromPtr = (IntPtr) (srcBuffer.ToInt64() + startSrcX + rowSrc*srcRowspan);

                toPtr = (IntPtr) (destBuffer.ToInt64() + startDstX + rowDst*dstRowspan);

                Win32.memcpy(toPtr, fromPtr, new UIntPtr((uint) copyWidth));
                rowDst++;
            }
        }


        void ReleaseUnmanagedObjects()
        {
            if(_internalBuffer!=IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_internalBuffer);
                _internalBuffer = IntPtr.Zero;
            }
            if (_tempScrollingBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_tempScrollingBuffer);
                _tempScrollingBuffer = IntPtr.Zero;
            }
        }

        void ReleaseManagedObjects()
        {
            
        }

        #region IDisposable implementation
        public void Dispose()
        {
            OnDispose();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
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

       
        ~FastBitmapSurface()
        {
            Dispose(false);
        }
        #endregion
    }
}
