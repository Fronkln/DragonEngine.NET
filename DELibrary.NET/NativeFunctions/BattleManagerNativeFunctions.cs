using PXD.CT;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonEngineLibrary.NativeFunctions
{
    internal partial class BattleManagerNativeFunctions
    {
        [DllImport("Y7Internal.dll", EntryPoint = "CBATTLE_MANAGER_GETCOLLISION2POINT")]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool GetCollision2Point(out CastResult result, ref Vector4 from, ref Vector4 to, uint targetMask);
    }
}
