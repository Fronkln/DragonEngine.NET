using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public struct DBBinaryFileHeader
    {
        [FieldOffset(0x0)]
        public FileHeader FileHeader;
        [FieldOffset(0x10)]
        public uint TablePointer;
    }
}
