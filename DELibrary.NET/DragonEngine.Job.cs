using System;
using System.Collections.Generic;
using System.Text;

namespace DragonEngineLibrary
{
    internal delegate void RegisterJobDelegate();
    internal delegate void RegisterWndProcDelegate(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

    internal static class JobManager
    {
        internal static List<JobRegisterInfo> _jobDelegates = new List<JobRegisterInfo>();
        internal static List<RegisterWndProcDelegate> _wndprocDelegates = new List<RegisterWndProcDelegate>();
    }

    internal class JobRegisterInfo
    {
        public Action funcRaw;
        public RegisterJobDelegate del;
        public IntPtr delPointer;
        public DEJob phase;
        public bool after;

        public JobRegisterInfo(Action func, RegisterJobDelegate del, IntPtr ptr, DEJob phase, bool after)
        {
            this.funcRaw = func;
            this.del = del;
            this.phase = phase;
            delPointer = ptr;
            this.after = after;
        }
    }
}
