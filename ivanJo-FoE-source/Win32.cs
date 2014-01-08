using System;
using System.Runtime.InteropServices;

namespace ForgeBot
{
    public class Win32
    {
        // memcpy - copy a block of memery
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

    }


}

