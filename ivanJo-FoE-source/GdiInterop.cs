using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ForgeBot
{
    public class GdiInterop
    {
        /// <summary>
        /// Enumeration for the raster operations used in BitBlt.
        /// In C++ these are actually #define. But to use these
        /// constants with C#, a new enumeration _type is defined.
        /// </summary>
        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, // dest = source
            SRCPAINT = 0x00EE0086, // dest = source OR dest
            SRCAND = 0x008800C6, // dest = source AND dest
            SRCINVERT = 0x00660046, // dest = source XOR dest
            SRCERASE = 0x00440328, // dest = source AND (NOT dest)
            NOTSRCCOPY = 0x00330008, // dest = (NOT source)
            NOTSRCERASE = 0x001100A6, // dest = (NOT src) AND (NOT dest)
            MERGECOPY = 0x00C000CA, // dest = (source AND pattern)
            MERGEPAINT = 0x00BB0226, // dest = (NOT source) OR dest
            PATCOPY = 0x00F00021, // dest = pattern
            PATPAINT = 0x00FB0A09, // dest = DPSnoo
            PATINVERT = 0x005A0049, // dest = pattern XOR dest
            DSTINVERT = 0x00550009, // dest = (NOT dest)
            BLACKNESS = 0x00000042, // dest = BLACK
            WHITENESS = 0x00FF0062, // dest = WHITE
        };

        public enum BICompressionType
        {
            BI_RGB = 0,
            BI_RLE8 = 1, 
            BI_RLE4 = 2, 
            BI_BITFIELDS = 3,
        };

        public enum DIBColorsType
        {
            DIB_RGB_COLORS = 0,
            DIB_PAL_COLORS = 1,
        };

        /// <summary>
        /// Enumeration to be used for those Win32 function 
        /// that return BOOL
        /// </summary>
        public enum Bool
        {
            False = 0,
            True
        };

        /// <summary>
        /// A BITMAPINFO stucture used to store an alphablended bitmap for adding to imagelist.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class BITMAPINFO
        {
            public Int32 biSize; // sizeof(BITMAPINFO)=40
            public Int32 biWidth; // bitmap height
            public Int32 biHeight; // bitmap width
            public Int16 biPlanes; // number of planes
            public Int16 biBitCount; // bits per pixel
            public Int32 biCompression; // of BICompressionType
            public Int32 biSizeImage; 
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public Int32 biClrUsed;
            public Int32 biClrImportant;
            //public Int32 colors;
        };


        /// <summary>
        /// Sets the background color.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="crColor">Color of the cr.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern int SetBkColor(IntPtr hdc, int crColor);

        /// <summary>
        /// CreateCompatibleDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        /// <summary>
        /// DeleteDC
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        /// <summary>
        /// SelectObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        /// <summary>
        /// DeleteObject
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// CreateCompatibleBitmap
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hObject, int width, int height);

        /// <summary>
        /// BitBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern bool BitBlt(HandleRef hDC, int x, int y, int nWidth, int nHeight,
                                         HandleRef hSrcDC, int xSrc, int ySrc, int dwRop); 

        /// <summary>
        /// StretchBlt
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool StretchBlt(IntPtr hObject, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hObjSource, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// SetStretchBltMode
        /// </summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool SetStretchBltMode(IntPtr hObject, int nStretchMode);

        /// <summary>
        /// Creates HBitmap
        /// </summary>
        /// <param name="hdc">DC</param>
        /// <param name="pbmi">BITMAPINFO structure</param>
        /// <param name="iUsage">of type DIBColorsType</param>
        /// <param name="bitmapBuffer">pointer to the buffer where we have bitmap raw data</param>
        /// <param name="hSection"></param>
        /// <param name="dwOffset"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In, MarshalAs(UnmanagedType.LPStruct)]BITMAPINFO pbmi, uint iUsage, out IntPtr bitmapBuffer, IntPtr hSection, uint dwOffset);

    }
}