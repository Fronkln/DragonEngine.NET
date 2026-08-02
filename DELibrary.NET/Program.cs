using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace DragonEngineLibrary
{
    public class Entry
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        protected static bool m_initOnce = false;
        private static HashSet<string> m_modsList = new HashSet<string>();

        public static string BaseDirectory;
        public static string Root;

        //Do whatever you want here
        static void ThreadTest()
        {
            try
            {
                DragonEngine.Log("Dragon Engine Library .NET Thread Start");
                File.WriteAllText("de_log.txt", "");

                DragonEngine.RefreshOffsets();

                while (!DragonEngine.IsEngineInitialized())
                {
                    DragonEngine.RefreshOffsets();
                }

                DragonEngine.Log("Dragon Engine initialized, initializing the library.");
                StartEngine();
            }
            catch (Exception ex)
            {
                DragonEngine.Log($"Failed to initialize\nError:{ex.Message}\n\nStacktrace:\n{ex.StackTrace}", Logger.Event.ERROR);
            }


        }

        private static void StartEngine()
        {
            DragonEngine.Log("Starting initializaton of all mods.");
            DragonEngine.Log($"Path: {AppDomain.CurrentDomain.BaseDirectory}");


            Thread modsThread = new Thread(LibThread);
            modsThread.Start();
        }

        public static void LibThread()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                string assemblyName = args.Name.Split(',')[0];

                // Resolve against an already-loaded assembly by simple name. Covers
                // DELibrary.NET and any binding a mod references that's already loaded,
                // and ignores version differences (a mod built against a slightly
                // different binding version than the one that ships).
                var loaded = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().Name == assemblyName);
                if (loaded != null)
                    return loaded;

                // Not loaded yet: the ImGui/ImPlot/ImNodes/ImGuizmo bindings and
                // HexaGen.Runtime ship as separate files next to DELibrary.NET now
                // that Costura no longer merges them in. A mod loaded from the mods
                // folder doesn't probe that directory, so load it from Root (the DE
                // Library folder where DELibrary.NET.dll and the bindings live).
                try
                {
                    if (!string.IsNullOrEmpty(Root))
                    {
                        string candidate = Path.Combine(Root, assemblyName + ".dll");
                        if (File.Exists(candidate))
                            return System.Reflection.Assembly.LoadFrom(candidate);
                    }
                }
                catch { }

                return null;
            };

            if (Directory.Exists("mods"))
            {
                int modCount = Parless.GetModCount();

                for (int i = 0; i < modCount; i++)
                {
                    IntPtr str = Parless.GetModName(i);
                    m_modsList.Add(Marshal.PtrToStringAnsi(str));
                }

                /*
                if (File.Exists("ModList.txt"))
                {
                    string[] mods = File.ReadAllText("ModList.txt").Split('|');
                    foreach (string mod in mods)
                        m_modsList.Add(mod.Substring(1));

                    foreach (var kv in m_modsList)
                        DragonEngine.Log(kv);
                }
                */

                string baseGameDirectoryAbsolute = Path.GetDirectoryName(Environment.ProcessPath);
                foreach (string modDirectoryRelative in Directory.GetDirectories(Path.Combine(baseGameDirectoryAbsolute, "mods")))
                {
                    string dirName = new DirectoryInfo(modDirectoryRelative).Name;

                    if (!m_modsList.Contains(dirName))
                        continue;

                    string configFile = Path.Combine(modDirectoryRelative, "de_mod.ini");

                    if (!File.Exists(configFile))
                        continue;

                    Ini ini = new Ini(configFile);
                    string dllFile = ini.GetValue("InitDll");
                    bool loadRes = ModManager.LoadMod(Path.Combine(baseGameDirectoryAbsolute, modDirectoryRelative, dllFile));

                    if (loadRes)
                        DragonEngine.Log($"Successfully loaded DLL library in {new DirectoryInfo(modDirectoryRelative).Name}");
                    else
                        DragonEngine.Log($"Failed to load DLL library in {new DirectoryInfo(modDirectoryRelative).Name}", Logger.Event.ERROR);
                }
            }

            DragonEngine.Log("All mods have been initialized.");
        }


        public static string GetModsDirectory()
        {
            return Path.Combine(BaseDirectory, "mods");
        }

        private static bool ShouldInitialize()
        {
            string modsDir = GetModsDirectory();

            if (!Directory.Exists(modsDir))
                return false;

            int modCount = Parless.GetModCount();

            if (modCount <= 0)
                return false;


            for(int i = 0; i < modCount; i++)
            {
                string modDir = Path.Combine(modsDir, Marshal.PtrToStringAnsi(Parless.GetModName(i)));

                if (!Directory.Exists(modDir))
                    continue;

                if (File.Exists(Path.Combine(modDir, "de_mod.ini")))
                    return true;
            }

            return false;
        }

        // This method will be called by native code inside the target process…
        public static int Initialize(IntPtr gameDirectory)
        {
            string directory = Marshal.PtrToStringUni(gameDirectory);
            //Initialize logging file
            //File.Create("dotnetlog.txt").Close();
            // DragonEngine._logStream = new MemoryStream();
            // DragonEngine._logWriter = new StreamWriter(DragonEngine._logStream);

            BaseDirectory = Environment.CurrentDirectory;
            Root = directory;

            //Create seperate thread for our C# library
            DragonEngine.Log("DragonEngine Library .Net Main Start");
            DragonEngine.Log($"BaseDirectory: {BaseDirectory}");
            DragonEngine.Log($"RootDirectory: {Root}");

            if(!ShouldInitialize())
            {
                DragonEngine.Log("There is no need to initialize the library. No compatible mods detected. Aborting.", Logger.Event.WARNING);
                return 0;
            }

            // Environment.CurrentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mods", "DE Library");
            DragonEngine.Initialize();

            Environment.CurrentDirectory = BaseDirectory;

            Thread thread1 = new Thread(ThreadTest);
            thread1.Start();

            return 0;
        }
    }
}
