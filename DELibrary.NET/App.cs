using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class App
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetHWNDDelegate();

#if YK2 || YLAD
        private static IntPtr _getHWNDPointer = DragonEngineLibrary.Unsafe.CPP.PatternSearch("48 8B 05 ? ? ? ? 48 85 C0 75 ? C3 48 8B 40");
#endif

#if IW
        private static IntPtr _getHWNDPointer = DragonEngineLibrary.Unsafe.CPP.ReadCall(
            DragonEngineLibrary.Unsafe.CPP.PatternSearch("E8 ? ? ? ? 48 8B C8 48 8D 54 24 ? FF 15 ? ? ? ? 44 8B 4F"));
#endif

#if YK2 || YLAD || IW
        private static GetHWNDDelegate _getHWND = Marshal.GetDelegateForFunctionPointer<GetHWNDDelegate>(_getHWNDPointer);


        /// <summary>
        /// Returns the game window handle.
        /// </summary>
        public static IntPtr HWND
        {
            get
            {
                return _getHWND();
            }
        }
#endif



        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool HasFocusDelegate();

#if YLAD || IW
        private static IntPtr _hasFocusPointer = DragonEngineLibrary.Unsafe.CPP.PatternSearch("48 8B 05 ? ? ? ? 48 85 C0 75 ? C3 8B 40 ? 83 E0");
#endif

#if YLAD || IW
        private static HasFocusDelegate _hasFocus = Marshal.GetDelegateForFunctionPointer<HasFocusDelegate>(_hasFocusPointer);


        /// <summary>
        /// Returns a <see cref="bool"/> indicating if the game window is focused.
        /// </summary>
        public static bool HasFocus
        {
            get
            {
                return _hasFocus();
            }
        }
#endif
    }
}
