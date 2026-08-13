using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECCameraBasic : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CECCAMERABASIC_GETTER_CAMERA_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_ECCameraBasic_Getter_CameraInfo(IntPtr cameraBasic);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CECCAMERABASIC_SETTER_CAMERA_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_ECCameraBasic_Setter_CameraInfo(IntPtr cameraBasic, IntPtr info);

        public CameraInfo CameraInfo
        {
            get => Marshal.PtrToStructure<CameraInfo>(DELibrary_ECCameraBasic_Getter_CameraInfo(Pointer));
            set
            {
                IntPtr ptr = value.ToIntPtr();
                DELibrary_ECCameraBasic_Setter_CameraInfo(Pointer, ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}
