using System.Runtime.InteropServices;

namespace PXD.CT
{
    [StructLayout(LayoutKind.Sequential, Size = 0x10)]
    public struct CasterData
    {
        public DragonEngineLibrary.Vector3 CasterPos;
        public float Distance;
    }
}
