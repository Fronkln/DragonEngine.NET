using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
#if YLAD_AND_UP
    public static class HActManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_GETTER_POINTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_HActManager_Getter_Pointer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACTPROC", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHActProc(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTNEXTHACT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestNextHAct(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTHACT", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_RequestHAct(ref HActRequestOptions opt);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_REQUESTPRELOAD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_HActManager_RequestPreload(uint talkID);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_GETBATTLESAFETYHACTRANGE", CallingConvention = CallingConvention.Cdecl)]
        public static extern Matrix4x4 GetBattleSafetyHActRange();


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_SKIP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_HActManager_Skip();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_ISPLAYING", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_IsPlaying();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_FINDRANGE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_HActManager_FindRange(ref Vector4 pos, HActRangeType type, ref HActRangeInfo outInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_HACTMANAGER_ISREQUESTORPLAYING", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        public static extern bool IsRequestOrPlaying();


        public static IntPtr Pointer { get { return DELib_HActManager_Getter_Pointer() ; } }

        /// <summary>
        /// Request HAct.
        /// </summary>
        public static bool RequestHActProc(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestHActProc(ref opt);
        }

        /// <summary>
        /// Request HAct.
        /// </summary>
        public static bool RequestHAct(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestHAct(ref opt);
        }

        /// <summary>
        /// Request HAct.
        /// </summary>
        public static bool RequestNextHAct(HActRequestOptions opt)
        {
            return DELib_HActManager_RequestNextHAct(ref opt);
        }

        /// <summary>
        /// Preload HAct.
        /// </summary>
        public static EntityHandle<AuthPlay> RequestPreload(TalkParamID id)
        {
            return DELib_HActManager_RequestPreload((uint)id);
        }

        ///<summary>Are any HActs playing?</summary>
        public static bool IsPlaying()
        {
            return DELib_HActManager_IsPlaying();
        }

        /// <summary>
        /// Skip the current HAct that is playing.
        /// </summary>
        public static void Skip()
        {
            DELib_HActManager_Skip();
        }

        public static bool FindRange(Vector4 pos, HActRangeType rangeType, ref HActRangeInfo outInf)
        {
            return DELib_HActManager_FindRange(ref pos, rangeType, ref outInf);
        }
    }
#endif
}
