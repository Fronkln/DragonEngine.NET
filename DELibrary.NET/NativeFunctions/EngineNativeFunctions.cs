using DragonEngineLibrary.Unsafe;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonEngineLibrary.NativeFunctions
{
    internal static unsafe class EngineNativeFunctions
    {
        public static delegate* unmanaged<IntPtr> GetCurrentSystemLanguageDir;

        public static void Init()
        {
#if YLAD
            GetCurrentSystemLanguageDir = (delegate* unmanaged<IntPtr>)CPP.ReadCall(CPP.PatternSearch("E8 ? ? ? ? 4C 8B C8 48 89 7C 24 ? 4C 8B C3 48 8D 15 ? ? ? ? 48 8D 4C 24"));
#endif
        }
    }
}
