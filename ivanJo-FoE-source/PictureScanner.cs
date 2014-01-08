using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ForgeBot
{
    /// <summary>
    /// Offers fast copy to buffer of image data to use instead of slow Image.GetPixel
    /// This class is NOT thread safe
    /// </summary>
    static class PictureScanner
    {

        private static IntPtr _rgbValues;
        private static int _height;
        private static int _width;
        private const int BytesPerPixel = 4;
        private static int _bufferLength = 0;

        /// <summary>
        /// Returns static memory "image" of 32bpp image passed as pointer
        /// </summary>
        /// <param name="sourceImageBuffer">IntPtr to the data buffer of image to scan</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        public static Screenshot MakeScreenshot(IntPtr sourceImageBuffer,int width,int height)
        {
            if (sourceImageBuffer == IntPtr.Zero)
                return new Screenshot(sourceImageBuffer,width,height,BytesPerPixel);

            _width = width;
            _height = height;
            
            // Declare an array to hold the bytes of the bitmap.
            int dataStride = _width*BytesPerPixel; // 32bpp image
            int totBytes = Math.Abs(dataStride) * _height;
            if (_rgbValues == IntPtr.Zero || _bufferLength != totBytes)
            {
                ReleaseUnmanagedBuffer();
                _rgbValues=Marshal.AllocHGlobal(totBytes);
                _bufferLength = totBytes;
            }

            Win32.memcpy(_rgbValues, sourceImageBuffer,
                                new UIntPtr((uint)_bufferLength));

            return new Screenshot(_rgbValues,_width,_height,BytesPerPixel);
        }

        /// <summary>
        /// Returns static memory "image" of 32bpp image passed as pointer
        /// </summary>
        /// <param name="sourceImage">Bitmap to scan</param>
        public static Screenshot MakeScreenshot(Bitmap sourceImage)
        {
            if (sourceImage == null)
                return new Screenshot(IntPtr.Zero, 0, 0, BytesPerPixel);

            _width = sourceImage.Width;
            _height = sourceImage.Height;

            // Declare an array to hold the bytes of the bitmap.
            int dataStride = _width * BytesPerPixel; // 32bpp image
            int totBytes = Math.Abs(dataStride) * _height;
            if (_rgbValues == IntPtr.Zero || _bufferLength != totBytes)
            {
                ReleaseUnmanagedBuffer();
                _rgbValues = Marshal.AllocHGlobal(totBytes);
                _bufferLength = totBytes;
            }

            // Lock data and copy image
            var bitmapData = sourceImage.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.WriteOnly,
                                                             sourceImage.PixelFormat);

            Win32.memcpy(bitmapData.Scan0, _rgbValues,
                                new UIntPtr((uint)(bitmapData.Stride * bitmapData.Height)));
            
            sourceImage.UnlockBits(bitmapData);

            return new Screenshot(_rgbValues, _width, _height, BytesPerPixel);
        }

        static void ReleaseUnmanagedBuffer()
        {
            if (_rgbValues != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_rgbValues);
                _bufferLength = 0;
            }
        }

    }
}
