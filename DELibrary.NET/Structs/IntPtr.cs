using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    [StructLayout(LayoutKind.Explicit, Size = 0x70)]
    public struct CameraInfo
    {
        [FieldOffset(0)] public Vector4 Pos;
        [FieldOffset(0x10)] public Vector4 Intr;
        [FieldOffset(0x20)] public Vector4 Up;
        [FieldOffset(0x30)] public Vector4 DofIntr;
        [FieldOffset(0x40)] public uint LookAtUID;
        [FieldOffset(0x44)] public float ClipNear;
        [FieldOffset(0x48)] public float ClipFar;
        [FieldOffset(0x4C)] public float FovyRad;
        [FieldOffset(0x50)] public float FovyHalfTangent;
        [FieldOffset(0x54)] public float FovyInvHalfTangent;
        [FieldOffset(0x58)] public float RollRad;
        [FieldOffset(0x5C)] public bool Ortho;
        [FieldOffset(0x60)] public float BlurRatio;
        [FieldOffset(0x64)] public float DofDistance;
    }
}
