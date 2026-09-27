using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonEngineLibrary
{
    namespace Unsafe
    {
        public static class CPP
        {
            [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNSAFE_ALLOC_BUFFER", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr AllocBuffer(IntPtr origin);

            /// <summary>
            /// Very dangerous to use, doesnt call constructors on deletion.
            /// </summary>
            /// <param name="text"></param>
            [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPP_FREE_MEM", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeUnmanagedMemory(IntPtr memory);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNSAFE_NOP", CallingConvention = CallingConvention.Cdecl)]
            public static extern void NopMemory(IntPtr memory, uint len);


            [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNSAFE_PATCH", CallingConvention = CallingConvention.Cdecl)]
            private static extern void Unsafe_NopMemory(IntPtr memory, IntPtr buf, int len);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_PATTERN_SEARCH", CallingConvention = CallingConvention.Cdecl)]
            private static extern IntPtr _PatternSearch(string pattern);

            public static IntPtr PatternSearch(string pattern)
            {
                IntPtr patternAddr = _PatternSearch(pattern);

                return patternAddr;
            }

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_READ_RELATIVE_ADDRESS", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ResolveRelativeAddress(IntPtr addr, int instructionLen);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_WRITE_RELATIVE_ADDRESS", CallingConvention = CallingConvention.Cdecl)]
            public static extern void WriteRelativeAddress(IntPtr addr, IntPtr target, int instructionLen);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_READ_CALL", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ReadCall(IntPtr addr);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_WRITE_CALL", CallingConvention = CallingConvention.Cdecl)]
            public static extern void WriteCall(IntPtr addr, IntPtr func);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_INJECT_HOOK", CallingConvention = CallingConvention.Cdecl)]
            public static extern void InjectHook(IntPtr addr, IntPtr func);

            [DllImport("Y7Internal.dll", EntryPoint = "LIB_TEST_FUNC", CallingConvention = CallingConvention.Cdecl)]
            public static extern int TestFunc();

            public static void PatchMemory(IntPtr addr, params byte[] bytes)
            {
                IntPtr byteArr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, byteArr, bytes.Length);

                Unsafe_NopMemory(addr, byteArr, bytes.Length);

                Marshal.FreeHGlobal(byteArr);
            }
        }
    }
}
