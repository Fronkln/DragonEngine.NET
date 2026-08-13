using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace DragonEngineLibrary.Mod
{
    internal class DragonEngineModLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        private readonly List<string> sharedAssemblies = new List<string>()
        {
            "DELibrary.NET",
            "DELibrary.NET.ImGui",
            "DELibrary.NET.ImGuizmo",
            "DELibrary.NET.ImNodes",
            "DELibrary.NET.ImPlot",
            "DELibrary.NET.ImPlot3D",
            "HexaGen.Runtime",
        };


        public DragonEngineModLoadContext(string name, string modPath)
            : base(name, isCollectible: true)
        {
            resolver = new AssemblyDependencyResolver(modPath);
        }


        protected override Assembly? Load(AssemblyName assemblyName)
        {
            // Use shared assemblies from the default context
            if (sharedAssemblies.Contains(assemblyName.Name))
                return null;

            string? path = resolver.ResolveAssemblyToPath(assemblyName);

            if (path != null)
            {
                byte[] bytes = File.ReadAllBytes(path);
                using var ms = new MemoryStream(bytes);
                return LoadFromStream(ms);
            }

            return null;
        }


        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? path = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

            if (path != null)
                return LoadUnmanagedDllFromPath(path);

            return IntPtr.Zero;
        }
    }
}
