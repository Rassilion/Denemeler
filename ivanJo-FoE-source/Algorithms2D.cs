using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Awesomium.Core;

namespace ForgeBot
{
    

    public interface ISetPixel
    {
        void SetPixel(Coordinate Coordinate);
    }

    #region Nested type: CSetPixel

    internal class CSetPixel : ISetPixel
    {
        private readonly Bitmap _image;
        public CSetPixel(Bitmap image)
        {
            _image = image;
        }

        #region ISetPixel Members

        public void SetPixel(Coordinate coordinate)
        {
            _image.SetPixel(coordinate.X, coordinate.Y, Color.White);
        }

        #endregion

    }

    #endregion
    public class Algorithms2D
    {
        #region Delegates

        public delegate void SetPixel(Coordinate Coordinate);

        #endregion

        public static void Line<TG>(Coordinate fromPoint, Coordinate toPoint, TG plotFunction) where TG : ISetPixel
        {
            int x0 = fromPoint.X;
            int y0 = fromPoint.Y;
            int x1 = toPoint.X;
            int y1 = toPoint.Y;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = -deltax/2;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1;
            else ystep = -1;
            for (int x = x0; x < x1; x++)
            {
                if (steep)
                {
                    plotFunction.SetPixel(new Coordinate(y, x));
                    fromPoint.X = y;
                    fromPoint.Y = x;
                }
                else
                {
                    plotFunction.SetPixel(new Coordinate(x, y));
                    fromPoint.X = x;
                    fromPoint.Y = y;
                }
                error = error + deltay;
                if (error > 0)
                {
                    y = y + ystep;
                    error = error - deltax;
                }
            }
        }

        private static void Swap(ref int p0, ref int p1)
        {
            var z = p0;
            p0 = p1;
            p1 = z;
        }

        
    }

    internal static class BitmapHelper
    {
        /// <summary>
        /// Copy 2 bitmas of a same size. Faster approach than using marshaling
        /// </summary>
        /// <param name="from">from image</param>
        /// <param name="to">to image</param>
        /// <param name="fromToPixelFormat">to/from pixel format of image. must be the same for both!</param>
        public static void CopyEqualSizeImages(this Bitmap from, Bitmap to,PixelFormat fromToPixelFormat)
        {
            if (from.PixelFormat != fromToPixelFormat && to.PixelFormat != fromToPixelFormat)
                throw new FormatException("Source/Target Picture has wrong PixelFormat");
            //if (from.Size != to.Size) throw new FormatException("Pictures are not Equal in Size");

            Rectangle lockRect = new Rectangle(0, 0, to.Width, to.Height);
            BitmapData toData = to.LockBits(lockRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            BitmapData fromData = from.LockBits(lockRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

            int srcStride = fromData.Stride;
            var count = new UIntPtr((uint)(srcStride * fromData.Height));

            Win32.memcpy(toData.Scan0, fromData.Scan0, count);

            to.UnlockBits(toData);
            from.UnlockBits(fromData);
        }

        /// <summary>
        /// Copy 2 bitmas of a different sizes
        /// </summary>
        /// <param name="from">from image</param>
        /// <param name="to">to image</param>
        /// <param name="fromToPixelFormat">to/from pixel format of image. must be the same for both!</param>
        public static void CopyTo(this Bitmap from, Bitmap to, PixelFormat fromToPixelFormat)
        {
            if (from.PixelFormat != fromToPixelFormat && to.PixelFormat != fromToPixelFormat)
                throw new FormatException("Source/Target Picture has wrong PixelFormat");
            if (from.Size == to.Size) 
            {
                from.CopyEqualSizeImages(to,fromToPixelFormat);
                return;
            }

            var toRectangle = new Rectangle(0, 0, from.Width, from.Height);
            BitmapData toData = to.LockBits(toRectangle, ImageLockMode.WriteOnly,
                                                fromToPixelFormat);
            BitmapData fromData = from.LockBits(toRectangle, ImageLockMode.ReadOnly,
                                                fromToPixelFormat);
            var data = new byte[fromData.Stride];
            for (int row = 0; row < fromData.Height; row++)
            {
                IntPtr fromPtr = (IntPtr)(fromData.Scan0.ToInt64() + row * toData.Stride);
                IntPtr toPtr = (IntPtr)(toData.Scan0.ToInt64() + row * toData.Stride);
                System.Runtime.InteropServices.Marshal.Copy(fromPtr, data, 0, data.Length);
                System.Runtime.InteropServices.Marshal.Copy(data, 0, toPtr, data.Length);
            }
            to.UnlockBits(toData);
            from.UnlockBits(fromData);
        }

        public static void CopyTo(this IntPtr from, IntPtr to, int width,int height,int bitsPerPixel)
        {
            var data = new byte[width*height*(bitsPerPixel/8)];
            for (int row = 0; row < height; row++)
            {
                IntPtr fromPtr = (IntPtr)(from.ToInt64() + row * data.Length);
                IntPtr toPtr = (IntPtr)(to.ToInt64() + row * data.Length);
                System.Runtime.InteropServices.Marshal.Copy(fromPtr, data, 0, data.Length);
                System.Runtime.InteropServices.Marshal.Copy(data, 0, toPtr, data.Length);
            }
        }

        /// <summary>
        /// Copy to bitmap from IntPtr data. images must be of the same size
        /// </summary>
        /// <param name="from">from image data (IntPtr)</param>
        /// <param name="to">to image</param>
        /// <param name="sWidth"></param>
        /// <param name="sHeight"></param>
        public static void CopyFromEqualSizeImage(this Bitmap to, IntPtr from,int sWidth,int sHeight)
        {
            if (to.Width != sWidth || to.Height!=sHeight)
            {
                throw new ApplicationException("Images are not of the same size");
            }

            var toRectangle = new Rectangle(0, 0, to.Width, to.Height);
            BitmapData toData = to.LockBits(toRectangle, ImageLockMode.WriteOnly,
                                                to.PixelFormat);
            Win32.memcpy(toData.Scan0, from, new UIntPtr((uint)(toData.Stride * to.Height)));

            to.UnlockBits(toData);
        }
    }

    public static class GraphicsHelper
    {
        /// <summary>
        /// Renders image to the graphics object. Slower overload.
        /// </summary>
        /// <param name="destinationGraphics"></param>
        /// <param name="image"></param>
        /// <param name="rectangleDst"></param>
        /// <param name="nXSrc"></param>
        /// <param name="nYSrc"></param>
        public unsafe static void GdiDrawImage(this Graphics destinationGraphics, Bitmap image, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            //var b = image.GetHbitmap();
            //var locked=image.LockBits(new Rectangle(0, 0, rectangleDst.Width, rectangleDst.Height),ImageLockMode.ReadWrite,image.PixelFormat);
            //GdiDrawImage2(destinationGraphics, b, rectangleDst, nXSrc, nYSrc);
            //GdiDrawImage(destinationGraphics, locked.Scan0, rectangleDst, nXSrc, nYSrc);
            //image.UnlockBits(locked);

            var gdiBitmap=image.GetHbitmap();
            GdiDrawImage(destinationGraphics, image.GetHbitmap(), rectangleDst, nXSrc, nYSrc);
            GdiInterop.DeleteObject(gdiBitmap);

            //NewGdiDrawImage(destinationGraphics, image, rectangleDst, nXSrc, nYSrc);
        }

        public unsafe static void GdiDrawImage2(this Graphics destinationGraphics, IntPtr sourceImagePtr, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            IntPtr targetDc = destinationGraphics.GetHdc();
            IntPtr memdc = GdiInterop.CreateCompatibleDC(targetDc);
            IntPtr bmp = sourceImagePtr;
            GdiInterop.SelectObject(memdc, bmp);
            GdiInterop.BitBlt(targetDc, rectangleDst.Left, rectangleDst.Top, rectangleDst.Width, rectangleDst.Height, memdc, nXSrc, nYSrc, GdiInterop.TernaryRasterOperations.SRCCOPY);
            GdiInterop.DeleteObject(bmp);
            GdiInterop.DeleteDC(memdc); 
            destinationGraphics.ReleaseHdc(targetDc);
        }


        public unsafe static void GdiDrawImage3(IntPtr targetDc, IntPtr sourceImagePtr, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            var memdc = GdiInterop.CreateCompatibleDC(targetDc);
            GdiInterop.SelectObject(memdc, sourceImagePtr);
            GdiInterop.BitBlt(targetDc, rectangleDst.Left, rectangleDst.Top, rectangleDst.Width, rectangleDst.Height, memdc, nXSrc, nYSrc, GdiInterop.TernaryRasterOperations.SRCCOPY);
            GdiInterop.DeleteObject(sourceImagePtr);
            GdiInterop.DeleteDC(memdc);
        }

        public unsafe static void NewGdiDrawImage(this Graphics destinationGraphics, Bitmap sourceImage, Rectangle destinationRect, int nXSrc, int nYSrc)
        {
            var sourceRect = new Rectangle(0, 0, destinationRect.Width, destinationRect.Height);

            BitmapData fromData = sourceImage.LockBits(sourceRect, ImageLockMode.ReadOnly,
                                                sourceImage.PixelFormat);
            IntPtr targetDc = destinationGraphics.GetHdc();
            IntPtr memdc = GdiInterop.CreateCompatibleDC(targetDc);
            IntPtr bmp = sourceImage.GetHbitmap();
            GdiInterop.SelectObject(memdc, bmp);

            try
            {
                byte[] data = new byte[fromData.Stride];
                for (int row = 0; row < fromData.Height; row++)
                {
                    var fromPtr = (IntPtr)(fromData.Scan0.ToInt64() + row * fromData.Stride);
                    var toPtr = (IntPtr)(memdc.ToInt64() + row * fromData.Stride);
                    System.Runtime.InteropServices.Marshal.Copy(fromPtr, data, 0, data.Length);
                    System.Runtime.InteropServices.Marshal.Copy(data, 0, toPtr, data.Length);
                }
            }
            finally
            {
                sourceImage.UnlockBits(fromData);
                GdiInterop.DeleteObject(bmp); 
                GdiInterop.DeleteDC(memdc);
                destinationGraphics.ReleaseHdc(targetDc);
            }
        }
        
        /// <summary>
        /// Renders image to the graphics object. Fast overload.
        /// </summary>
        /// <param name="destinationGraphics"></param>
        /// <param name="sourceImagePtr"></param>
        /// <param name="rectangleDst"></param>
        /// <param name="nXSrc"></param>
        /// <param name="nYSrc"></param>
        public unsafe static void GdiDrawImage(this Graphics destinationGraphics, IntPtr sourceImagePtr, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            IntPtr targetDc = destinationGraphics.GetHdc();
            IntPtr memdc = GdiInterop.CreateCompatibleDC(targetDc);
            GdiInterop.SelectObject(memdc, sourceImagePtr);
            GdiInterop.BitBlt(targetDc, rectangleDst.Left, rectangleDst.Top, rectangleDst.Width, rectangleDst.Height, memdc, nXSrc, nYSrc, GdiInterop.TernaryRasterOperations.SRCCOPY);
            //GdiInterop.DeleteObject(sourceImagePtr);
            GdiInterop.DeleteDC(memdc);
            destinationGraphics.ReleaseHdc(targetDc);
        }

        public unsafe static void GdiDrawImage(this Graphics destinationGraphics, Graphics sourceGraphics, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            IntPtr targetDc = destinationGraphics.GetHdc();
            var refTargetDc = new HandleRef(destinationGraphics, targetDc);

            RenderInternal(refTargetDc, sourceGraphics, rectangleDst, nXSrc, nYSrc);
        }

        /// <summary>
        /// Copy from image IntPtr to a bitmap object
        /// </summary>
        /// <param name="ptrImageSource"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="targetImage"></param>
        public unsafe static void CopyIntPtr2Bitmap(IntPtr ptrImageSource, int width, int height, Bitmap targetImage)
        {
            var rect = new Rectangle(0, 0, width, height);
            int bytes = width * height * 4;
            // copy to destination image object
            var bmpData = targetImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            // Get the address of the first line of image.
            IntPtr ptrImageTarget = bmpData.Scan0;
            Win32.memcpy(ptrImageTarget, ptrImageSource, new UIntPtr((uint)bytes));
            targetImage.UnlockBits(bmpData);
        }

        public unsafe static IntPtr GetIntPtrFromImage(Bitmap image)
        {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var bmpData = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);
            IntPtr ptrImageTarget = bmpData.Scan0;
            image.UnlockBits(bmpData);
            return ptrImageTarget;
        }
        /// <INCLUDE file="doc\BufferedGraphics.uex" path='docs/doc[@for="BufferedGraphics.RenderInternal"]/*' /> 
        /// <DEVDOC>  
        ///         Internal method that renders the specified buffer into the target. 
        /// </DEVDOC>  
        private unsafe static void RenderInternal(HandleRef refTargetDC, Graphics sourceBuffer, Rectangle rectangleDst, int nXSrc, int nYSrc)
        {
            IntPtr sourceDC = sourceBuffer.GetHdc();

            try
            {
                GdiInterop.BitBlt(refTargetDC, rectangleDst.X, rectangleDst.Y, rectangleDst.Width, rectangleDst.Height,
                                         new HandleRef(sourceBuffer, sourceDC), nXSrc, nYSrc, (int)GdiInterop.TernaryRasterOperations.SRCCOPY);
            }
            finally
            {
                sourceBuffer.ReleaseHdc(sourceDC);
            }
        }

    }
}