using DragonEngineLibrary.NativeFunctions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DragonEngineLibrary
{
    internal static class NativeFunction
    {
        public static void Init()
        {
            EngineNativeFunctions.Init();
        }
    }
}
