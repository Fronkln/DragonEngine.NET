using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class CameraBase : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCAMERA_BASE_SLEEP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_CameraBase_Sleep(IntPtr camera);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCAMERA_BASE_GETTER_CAMERA_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_CameraBase_Getter_CameraInfo(IntPtr cameraBasic);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CCAMERA_BASE_SETTER_CAMERA_INFO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_CameraBase_Setter_CameraInfo(IntPtr cameraBasic, IntPtr info);

        public CameraInfo CameraInfo
        {
            get => Marshal.PtrToStructure<CameraInfo>(DELibrary_CameraBase_Getter_CameraInfo(Pointer));
            set
            {
                IntPtr ptr = value.ToIntPtr();
                DELibrary_CameraBase_Setter_CameraInfo(Pointer, ptr);
                Marshal.FreeHGlobal(ptr);
            }
        }

        ///<summary>Disable the camera.</summary>
        public void Sleep()
        {
            DELib_CameraBase_Sleep(Pointer);
        }

        public ECCameraBasic Basic => GetComponent<ECCameraBasic>(ECSlotID.camera_basic);
    }
}
