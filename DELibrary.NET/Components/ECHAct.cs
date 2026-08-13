using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECHAct : ECCharaComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_HACT_GET_PLAY_INFO", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ECHAct_GetPlayInfo(IntPtr hact, uint type, ref HActRangeInfo inInf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_HACT_SETUP_FIND_OPTION", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELibrary_ECHAct_SetupFindOption(IntPtr hact, ref FindOption option);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_HACT_UPDATE_RESULT", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal static extern void DELibrary_ECHAct_UpdateResult(IntPtr hact, ref FindOption option);


        [StructLayout(LayoutKind.Sequential, Size = 0x40)]
        public struct FindOption
        {
            public Vector4 pos_;
            public Quaternion ground_rot_;
            public uint range_;
            public uint idx_;
            public uint grid_idx_;
            public float search_length_;
            public byte search_type_;
            public bool is_searching_idx_;
        };


        public unsafe bool GetPlayInfo(ref HActRangeInfo input, HActRangeType type)
        {
            bool result = DELibrary_ECHAct_GetPlayInfo(Pointer, (uint)type, ref input);
            return result;
        }

        public bool SetupFindOption(ref FindOption option)
        {
            return DELibrary_ECHAct_SetupFindOption(Pointer, ref option);
        }

        public HActRangeInfo UpdateResult(FindOption option)
        {
            unsafe
            {
                HActRangeInfo* inf = (HActRangeInfo*)&option;
                DELibrary_ECHAct_UpdateResult(Pointer, ref option);

                return *inf;
            }    
        }
    }
}
