using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class ParticleManager
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_PLAY1", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint DELib_ParticleManager_Play(ParticleID particleID, IntPtr matrix, ParticleType type);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_IS_LOADED_RESOURCE_RAW", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U1)]
        internal extern static bool DELib_ParticleManager_IsLoadedRaw(ParticleID particleID);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_LOAD_RAW", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void DELib_ParticleManager_LoadRaw(ParticleID particleID, bool isEvent);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_UNLOAD_RAW", CallingConvention = CallingConvention.Cdecl)]
        internal extern static void DELib_ParticleManager_UnloadRaw(ParticleID particleID);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_GET_CMN_ARCHIVE", CallingConvention = CallingConvention.Cdecl)]
        public extern static EntityHandle<ParticleResource> GetCmnArchive();

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_MANAGER_GET_RAW_ARCHIVE", CallingConvention = CallingConvention.Cdecl)]
        public extern static EntityHandle<ParticleResource> GetRawArchive();

        ///<summary>Play a particle.</summary>
        public static EntityHandle<ParticleInterface> Play(ParticleID particleID, Matrix4x4 matrix, ParticleType type)
        {
            IntPtr matrixMem = matrix.ToIntPtr();
            uint result = DELib_ParticleManager_Play(particleID, matrixMem, type);

            return result;
        }

        ///<summary>Is the particle loaded?</summary>
        public static bool IsLoadedRaw(ParticleID particleID)
        {
            return DELib_ParticleManager_IsLoadedRaw(particleID);
        }

        public static void LoadRaw(ParticleID particleID, bool isEvent)
        {
            DELib_ParticleManager_LoadRaw(particleID, isEvent);
        }

        public static void UnloadRaw(ParticleID particleID)
        {
            DELib_ParticleManager_UnloadRaw(particleID);
        }
    }
}
