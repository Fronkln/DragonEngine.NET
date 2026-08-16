using System;

namespace DragonEngineLibrary
{
    public unsafe class FileRaw : UnmanagedObject
    {
        public FileRaw(IntPtr pointer)
        {
            Pointer = pointer;
        }

        public IntPtr GetBuffer()
        {
            if (Pointer == IntPtr.Zero)
                return IntPtr.Zero;

            IntPtr cslBuffer = *(IntPtr*)(Pointer + 0x50);

            if (cslBuffer == IntPtr.Zero)
                return IntPtr.Zero;

            return *(IntPtr*)(cslBuffer + 0x20);
        }
    }
}
