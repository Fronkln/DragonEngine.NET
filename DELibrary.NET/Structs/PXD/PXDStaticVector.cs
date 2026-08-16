using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Unsafe. Use with caution or risk a crash!
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public unsafe struct PXDStaticVector
    {
        public IntPtr MPElement;
        public uint VectorSize;
        public uint ElementSize;

        public uint ElementAtU32(int idx)
        {
            uint* arr = (uint*)MPElement;
            return arr[idx];
        }

        public T ElementAt<T>(int idx) where T: unmanaged
        {
            T* arr = (T*)MPElement;
            return arr[idx];
        }

        public T ElementAtUnmanagedObject<T>(int idx) where T : UnmanagedObject, new()
        {
            IntPtr* arr = (IntPtr*)MPElement;
            return new T() { Pointer = arr[idx] };
        }


        public uint[] GetArrayU32()
        {
            uint[] array = new uint[ElementSize];

            for (int i = 0; i < array.Length; i++)
                array[i] = ElementAtU32(i);

            return array;
        }
    }
}
