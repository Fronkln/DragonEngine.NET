using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonEngineLibrary
{
    public class ParticleInterface : EntityBase
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_PAUSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ParticleInstance_Pause(IntPtr ptcPtr, bool pause);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_FORCE_LOOP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ParticleInstance_ForceLoop(IntPtr ptcPtr, bool loop);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_GET_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DELib_ParticleInstance_GetTick(IntPtr ptcPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_SET_TICK", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ParticleInstance_SetTick(IntPtr ptcPtr, int tick);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_GET_TICK_SCALE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_ParticleInstance_GetTickScale(IntPtr ptcPtr);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CPARTICLE_INSTANCE_SET_TICK_SCALE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ParticleInstance_SetTickScale(IntPtr ptcPtr, float scale);

        public void Resume() => DELib_ParticleInstance_Pause(Pointer, false);
        public void Pause() => DELib_ParticleInstance_Pause(Pointer, true);

        public void SetPlaying(bool playing) => DELib_ParticleInstance_Pause(Pointer, !playing);

        public void SetLoop(bool looping) => DELib_ParticleInstance_ForceLoop(Pointer, looping);

        public int GetTick() => DELib_ParticleInstance_GetTick(Pointer);
        public void SetTick(int tick) => DELib_ParticleInstance_SetTick(Pointer, tick);

        public float GetTickScale() => DELib_ParticleInstance_GetTickScale(Pointer);
        public void SetTickScale(float scale) => DELib_ParticleInstance_SetTickScale(Pointer, scale);
    }
}
