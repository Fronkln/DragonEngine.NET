using DragonEngineLibrary.Mod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DragonEngineLibrary
{
    /// <summary>
    /// Manages the loading/unloading of <see cref="DragonEngineMod"/> assemblies and provides information about currently loaded mods.
    /// </summary>
    public static class ModManager
    {
        internal static Dictionary<string, DragonEngineModLoadContext> LoadedModContexts = new Dictionary<string, DragonEngineModLoadContext>();
        internal static Dictionary<DragonEngineModLoadContext, Assembly> LoadedModEntryAssemblies = new Dictionary<DragonEngineModLoadContext, Assembly>();
        internal static Dictionary<DragonEngineModLoadContext, string> LoadedModEntryAssemblyPaths = new Dictionary<DragonEngineModLoadContext, string>();


        /// <summary>
        /// Loads the specified mod into the game and calls its <see cref="DragonEngineMod.OnModInit"/> function.
        /// </summary>
        /// <param name="path">The path to the main mod DLL.</param>
        /// <returns>Returns <see langword="true"/> if the operation was successful, otherwise <see langword="false"/>.</returns>
        public static bool LoadMod(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!File.Exists(path))
            {
                DragonEngine.Log(Directory.GetCurrentDirectory(), Logger.Event.DEBUG);
                DragonEngine.Log($"{path} does not exist.", Logger.Event.ERROR);
                return false;
            }

            string? modName = new FileInfo(path).Directory?.Name;
            DragonEngineModLoadContext modAssemblyLoadContext = new DragonEngineModLoadContext(modName, path);
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                using var ms = new MemoryStream(bytes);
                Assembly loadedAssembly = modAssemblyLoadContext.LoadFromStream(ms);

                Type modInfoType = typeof(DEModInfo);

                foreach (CustomAttributeData customAttr in loadedAssembly.CustomAttributes)
                {
                    if (customAttr.AttributeType.FullName == typeof(DEModInfo).FullName) // Compare names since type comparison can be unreliable
                    {
                        return ProcessDEMod(path, customAttr.ConstructorArguments, modAssemblyLoadContext, loadedAssembly);
                    }
                }
                modAssemblyLoadContext.Unload();
                return false;
            }
            catch (Exception ex)
            {
                //It was a valid C# Dragon Engine .NET library but there was an error
                if (ex as BadImageFormatException == null)
                {
                    if (ex as FileLoadException != null && ex.InnerException as NotSupportedException != null)
                        DragonEngine.MessageBox((IntPtr)0, $"Failed to load {Path.GetFileName(path)} in mods/{Path.GetDirectoryName(path)} because it was untrusted by system, please unblock!\n" +
                            $"1)Go to the problematic file\n" +
                            $"2)Right click on it, go to properties\n" +
                            $"3)Press the unblock button", "Load Error", 0);
                    else
                    {
                        DragonEngine.Log($"Failed to load library, Exception type: {ex.ToString()}\n\nStacktrace:\n{Environment.StackTrace}\n\nMessage:\n {ex.Message}\n\nInnerException:\n{ex.InnerException}", Logger.Event.ERROR);
                        DragonEngine.MessageBox(IntPtr.Zero, "Failed to load mod. Information has been logged to de_log.txt (where the exe is)", "Load Error", 0);
                    }
                }
                modAssemblyLoadContext.Unload();
                return false;
            }
        }


        internal static bool ProcessDEMod(string path, IList<CustomAttributeTypedArgument> modInfo, DragonEngineModLoadContext modLoadContext, Assembly mainAssembly)
        {
            string modName = (string)modInfo[0].Value;
            Type modType = (Type)modInfo[1].Value;

            if (modType == null)
            {
                DragonEngine.Log($"The mod {modName} does not have a valid mod initialization class", Logger.Event.ERROR);
                modLoadContext.Unload();
                return false;
            }


            if (modType.BaseType.FullName == "DragonEngineLibrary.DragonEngineMod")
            {
                DragonEngineMod deModObject = (DragonEngineMod)Activator.CreateInstance(modType);

                if (deModObject != null)
                {
                    if (LoadedModContexts.ContainsKey(modName))
                    {
                        DragonEngine.Log($"Cannot load mod. A mod named '{modName}' is already loaded.", Logger.Event.ERROR);
                        modLoadContext.Unload();
                        return false;
                    }

                    deModObject.ModPath = new FileInfo(path).Directory.FullName;
                    LoadedModContexts.Add(modName, modLoadContext);
                    LoadedModEntryAssemblies.Add(modLoadContext, mainAssembly);
                    LoadedModEntryAssemblyPaths.Add(modLoadContext, path);
                    deModObject.OnModInit();
                    DragonEngine.Log($"Loaded mod '{modName}'.", Logger.Event.INFORMATION);
                    return true;
                }
                else
                {
                    modLoadContext.Unload();
                    DragonEngine.Log("Mod class initialization failed!", Logger.Event.ERROR);
                    return false;
                }
            }
            else
            {
                modLoadContext.Unload();
                DragonEngine.Log($"{modName}'s initialization class does not derive from DragonEngineMod!", Logger.Event.ERROR);
                return false;
            }
        }


        /// <summary>
        /// Gets a list of currently loaded mods.
        /// </summary>
        public static List<string> GetLoadedMods()
        {
            List<string> mods = new List<string>();
            foreach (string key in LoadedModContexts.Keys)
            {
                mods.Add(key);
            }
            return mods;
        }


        /// <summary>
        /// Gets a list of loaded assemblies from a specific mod.
        /// </summary>
        /// <param name="modName">The mod to look for.</param>
        public static IReadOnlyList<Assembly> GetModAssemblies(string modName)
        {
            List<Assembly> modAssemblies = new List<Assembly>();
            if (LoadedModContexts.ContainsKey(modName))
            {
                foreach (Assembly assembly in LoadedModContexts[modName].Assemblies)
                {
                    modAssemblies.Add(assembly);
                }
            }
            return modAssemblies.AsReadOnly();
        }


        /// <summary>
        /// Gets a loaded mod's main assembly.
        /// </summary>
        /// <param name="modName">The mod to look for.</param>
        public static Assembly GetModMainAssembly(string modName)
        {
            if (LoadedModContexts.ContainsKey(modName))
            {
                return LoadedModEntryAssemblies[LoadedModContexts[modName]];
            }
            return null;
        }


        /// <summary>
        /// Gets a loaded mod's main assembly path.
        /// </summary>
        /// <param name="modName">The mod to look for.</param>
        public static string GetModMainAssemblyPath(string modName)
        {
            if (LoadedModContexts.ContainsKey(modName))
            {
                return LoadedModEntryAssemblyPaths[LoadedModContexts[modName]];
            }
            return "";
        }


        /// <summary>
        /// Unloads a specific mod from the game, first calling its <see cref="DragonEngineMod.OnModUnload"/> function.
        /// </summary>
        /// <param name="modName">The mod to unload.</param>
        /// <returns>Returns <see langword="true"/> if the operation was successful, otherwise <see langword="false"/>.</returns>
        public static bool UnloadMod(string modName)
        {
            if (LoadedModContexts.ContainsKey(modName))
            {
                DragonEngineModLoadContext modLoadContext = LoadedModContexts[modName];
                Assembly mainAssembly = LoadedModEntryAssemblies[modLoadContext];
                Type modInfoType = typeof(DEModInfo);

                foreach (CustomAttributeData customAttr in mainAssembly.CustomAttributes)
                {
                    if (customAttr.AttributeType.FullName == typeof(DEModInfo).FullName) // Compare names since type comparison can be unreliable
                    {
                        Type modType = (Type)customAttr.ConstructorArguments[1].Value;
                        if (modType.BaseType.FullName == "DragonEngineLibrary.DragonEngineMod")
                        {
                            DragonEngineMod deModObject = (DragonEngineMod)Activator.CreateInstance(modType);
                            if (deModObject.OnModUnload())
                            {
                                modLoadContext.Unload();
                                LoadedModEntryAssemblies.Remove(modLoadContext);
                                LoadedModEntryAssemblyPaths.Remove(modLoadContext);
                                LoadedModContexts.Remove(modName);
                                DragonEngine.Log($"Unloaded mod '{modName}'.", Logger.Event.INFORMATION);
                                return true;
                            }
                        }
                    }
                }
            }

            DragonEngine.Log($"Could not unload mod '{modName}'.", Logger.Event.ERROR);
            return false;
        }


        /// <summary>
        /// Reloads a specific mod. It will attempt to call <see cref="UnloadMod"/> followed by <see cref="LoadMod"/>.
        /// </summary>
        /// <remarks>It is important that the mod remains in the same directory and retains the same entry DLL name.</remarks>
        /// <param name="modName">The mod to reload.</param>
        /// <returns>Returns <see langword="true"/> if the operation was successful, otherwise <see langword="false"/>.</returns>
        public static bool ReloadMod(string modName)
        {
            string mainAssemblyPath = GetModMainAssemblyPath(modName);
            if (!string.IsNullOrEmpty(mainAssemblyPath))
            {
                if (UnloadMod(modName))
                    return LoadMod(mainAssemblyPath);
            }

            return false;
        }
    }
}
