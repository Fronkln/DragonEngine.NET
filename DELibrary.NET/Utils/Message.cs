using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary.Utils
{
    internal static class Message
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBox(IntPtr handle, string text, string title, int type);
    }
}
