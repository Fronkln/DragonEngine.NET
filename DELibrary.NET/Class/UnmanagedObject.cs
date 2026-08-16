using System;

namespace DragonEngineLibrary
{
    public class UnmanagedObject
    {
        public IntPtr Pointer;

        public UnmanagedObject()
        {

        }

        public UnmanagedObject(IntPtr pointer)
        {
            Pointer = pointer;
        }
    }
}
