
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct FileHeader
    {
        public uint TagId;
        public char PlatformId;
        public char Endian;
        public char SizeExtend;
        public char Relocated;
        public uint Version;
        public uint Size;
    }
}
