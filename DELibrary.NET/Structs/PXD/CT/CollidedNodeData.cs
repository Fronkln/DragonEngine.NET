using System.Runtime.InteropServices;

namespace PXD.CT
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct CollidedNodeData
    {
        public DragonEngineLibrary.Vector3 CollidedPos;
        public uint CollidedNodeUID;
    }
}
