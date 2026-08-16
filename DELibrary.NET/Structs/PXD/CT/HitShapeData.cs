using System.Runtime.InteropServices;

namespace PXD.CT
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct HitShapeData
    {
        public uint Status;
        public uint CollidedShapeType;
        public uint CollidedShapeAttribute;
        public int CollectorShapeIndex;
    }
}
