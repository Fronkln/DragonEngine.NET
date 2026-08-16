using System.Runtime.InteropServices;

namespace PXD.CT
{
    [StructLayout(LayoutKind.Sequential, Size = 0x40)]
    public struct CastResult
    {
        public HitShapeData hitShapeData;
        public CollidedNodeData hitPos;
        public CollidedNodeData hitNormal;
        public CasterData CasterData;
    }
}
