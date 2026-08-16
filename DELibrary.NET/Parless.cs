using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DragonEngineLibrary
{
    public class Parless
    {
        [DllImport("YakuzaParless.asi", EntryPoint = "YP_GET_NUM_MODS", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetModCount();

        [DllImport("YakuzaParless.asi", EntryPoint = "YP_GET_MOD_NAME", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetModName(int idx);
        [DllImport("YakuzaParless.asi", EntryPoint = "YP_GET_GAME_NAME", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGameName();

        public static string[] GetModsList()
        {
            int count = GetModCount();

            List<string> names = new List<string>();

            for (int i = 0; i < count; i++)
                names.Add(Marshal.PtrToStringAnsi(GetModName(i)));

            return names.ToArray();
        }
    }
}
