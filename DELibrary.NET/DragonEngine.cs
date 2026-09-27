using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Security;

namespace DragonEngineLibrary
{
    public static class DragonEngine
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_INIT", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_Init();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_ATTACKER_OVERRIDE_FUNCTION", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DELib_RegisterAttackerOverrideFunc(IntPtr deleg);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ENGINE_INITIALIZED", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsEngineInitialized();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_REFRESH_OFFSETS", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_RefreshOffsets();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_DELTATIME", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetDeltaTime();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_GET_FRAMERATE", CallingConvention = CallingConvention.Cdecl)]
        private static extern float DELib_GetFrameRate();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_SET_SPEED", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_SetSpeed(DESpeedType speedType, float speed);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_HUMAN_PLAYER", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_GetHumanPlayer();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_ALLOW_ALT_TAB_PAUSE", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_AllowAltTabPause(bool allow);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_FORCE_SET_CURSOR_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        private static extern void DELib_ForceSetCursorVisible(bool allow);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_CURSOR_FORCED_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsCursorForcedVisible();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_DRAGONENGINE_IS_ALT_TAB_PAUSE_ALLOWED", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        private static extern bool DELib_IsAltTabPauseAllowed();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterJob(IntPtr deleg, DEJob type, bool after);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UNREGISTER_DE_JOB", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_UnregisterJob(IntPtr deleg, DEJob type);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_REGISTER_WNDPROC", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint DELib_RegisterWndProc(IntPtr deleg);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string libname);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr handle, string text, string title, int type);

        /// <summary>
        /// Initialize Dragon Engine library. Important for it to properly function.
        /// </summary>
        /// 

        [SecurityCritical, HandleProcessCorruptedStateExceptions]
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;  // this is important. Any exception occuring in the logging mechanism can cause a stack overflow exception which triggers the window's own JIT message/App crash message if Win JIT is not available.

            Exception ex = e.ExceptionObject as Exception;
            DragonEngine.Log($"***************FATAL ERROR***************\nInner Exception:\n{ex.InnerException}\n\nMessage:\n{ex.Message}\n\nStacktrace:\n{ex.StackTrace}", Logger.Event.FATAL);
            MessageBox((IntPtr)0, "Fatal error! More information available on de_log.txt (where game exe is located). The game will now exit", "Fatal DELibrary Error", 0x00000010);
            Environment.Exit(-1); // exit and avoid WER etc
        }

        public static string SystemLanguageDir
        {
            get
            {
                unsafe
                {
                    return Marshal.PtrToStringAnsi(NativeFunctions.EngineNativeFunctions.GetCurrentSystemLanguageDir());
                }
            }
        }


        public static bool IsGOG { get; private set; }

        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Stopwatch initTime = Stopwatch.StartNew();

            string libPath = Path.Combine(Entry.Root, "Y7Internal.dll");
            DragonEngine.Log($"Y7Internal path: {libPath}");

            DragonEngine.Log("Pre Y7Internal.dll import");

            if (!File.Exists(libPath))
            {
                DragonEngine.Log("Y7Internal could not be found!", Logger.Event.FATAL);
                return;
            }

            Stopwatch loadTime = Stopwatch.StartNew();

            if (LoadLibrary(libPath) == IntPtr.Zero)
                DragonEngine.Log($"Failed to load the library! GetLastWinError32: {Marshal.GetLastWin32Error()}", Logger.Event.FATAL);

            DragonEngine.Log($"Library load time: {initTime.Elapsed.TotalSeconds}");

            /*
            string cimguiPath = Path.Combine(new FileInfo(libPath).Directory.FullName, "cimgui.dll");

            if (File.Exists(cimguiPath))
                LoadLibrary(cimguiPath);
            */

            DragonEngine.Log("Y7Internal loaded. Going to initialize");

            DELib_Init();

            DragonEngine.Log($"DELib load and init time: {initTime.Elapsed.TotalSeconds}");

            Environment.CurrentDirectory = Path.Combine(Entry.BaseDirectory);

#if TURN_BASED_GAME
            BattleTurnManager.OverrideAttackerSelectionInfo.deleg = new BattleTurnManager.OverrideAttackerSelectionDelegate(BattleTurnManager.ReturnManualAttackerSelectionResult);
            BattleTurnManager.OverrideAttackerSelectionInfo.delegPtr = Marshal.GetFunctionPointerForDelegate(BattleTurnManager.OverrideAttackerSelectionInfo.deleg);
            DELib_RegisterAttackerOverrideFunc(BattleTurnManager.OverrideAttackerSelectionInfo.delegPtr);
#endif

            IsGOG = GetModuleHandle("galaxy64") != IntPtr.Zero;

            EngineHooks.Initialize();
        }


        internal static void LibUpdate()
        {
        }

        /// <summary>
        /// This is not related to DragonEngine.Initialize
        /// </summary>
        public static bool IsEngineInitialized()
        {
            return DELib_IsEngineInitialized();
        }


        /// <summary>
        /// Sends a message to the <see cref="Logger"/>. This will be printed to the console by default, if one is available.
        /// </summary>
        /// <param name="value">The contents of the message.</param>
        public static void Log(object value)
        {
            string valueStr = value.ToString();
            Logger.LogEvent(valueStr, Assembly.GetCallingAssembly().GetName().Name, Logger.Event.INFORMATION);
        }


        /// <summary>
        /// Sends a message to the <see cref="Logger"/>. This will be printed to the console by default, if one is available.
        /// </summary>
        /// <param name="value">The contents of the message.</param>
        /// <param name="eventType">The event type.</param>
        public static void Log(object value, Logger.Event eventType)
        {
            string valueStr = value.ToString();
            Logger.LogEvent(valueStr, Assembly.GetCallingAssembly().GetName().Name, eventType);
        }

        public static bool IsCursorForcedVisible()
        {
            return DELib_IsCursorForcedVisible();
        }

        public static void ForceSetCursorVisible(bool visible)
        {
            DELib_ForceSetCursorVisible(visible);
        }

        public static void AllowAltTabPause(bool allow)
        {
            DELib_AllowAltTabPause(allow);
        }

        public static bool IsAltTabPauseAllowed()
        {
            return DELib_IsAltTabPauseAllowed();
        }

        /// <summary>
        /// Register a function that will be executed by Dragon Engine.
        /// </summary>
        /// <param name="after">If after is set to true, it will execute after main game functions.</param>
        public static void RegisterJob(Action action, DEJob jobID, bool after = false)
        {
            RegisterJobDelegate del = new RegisterJobDelegate(action);
            JobRegisterInfo inf = new JobRegisterInfo(action, del, Marshal.GetFunctionPointerForDelegate(del), jobID, after);
            JobManager._jobDelegates.Add(inf);

            DELib_RegisterJob(inf.delPointer, jobID, after);
        }

        public static void RegisterWndProc(Action<IntPtr, int, IntPtr, IntPtr> func)
        {
            RegisterWndProcDelegate del = new RegisterWndProcDelegate(func);
            DELib_RegisterWndProc(Marshal.GetFunctionPointerForDelegate(del));

            JobManager._wndprocDelegates.Add(del);
        }


        public static void RefreshOffsets()
        {
            DELib_RefreshOffsets();
        }

        /// <summary>
        /// Unregister a job that was registered.
        /// </summary>
        public static void UnregisterJob(Action func, DEJob phase)
        {

            foreach (JobRegisterInfo job in JobManager._jobDelegates.ToArray())
                if (job.phase == phase)
                    if (job.funcRaw == func)
                    {
                        DELib_UnregisterJob(job.delPointer, phase);
                    }
        }

        public static bool IsKeyDown(VirtualKey virtualKey)
        {
            return (GetAsyncKeyState((int)virtualKey)) == -32767;
        }

        public static bool IsKeyHeld(VirtualKey virtualKey)
        {
            return (GetAsyncKeyState((int)virtualKey) & 0x8000) == 0x8000;
        }


        public static float deltaTime
        {
            get
            {
                return DELib_GetDeltaTime();
            }
        }

        /// <summary>
        /// Current FPS (Frames per second) of the engine.
        /// </summary>
        public static float frameRate
        {
            get
            {
                return DELib_GetFrameRate();
            }
        }

        public static void SetSpeed(DESpeedType speedType, float speed)
        {
            DELib_SetSpeed(speedType, speed);
        }

        //Same as GetSceneEntity(SceneEntity.human_player)
        public static Character GetHumanPlayer()
        {
            return new EntityHandle<Character>(DELib_GetHumanPlayer());
        }
    }
}
