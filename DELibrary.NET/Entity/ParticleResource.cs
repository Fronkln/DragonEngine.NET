using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public unsafe class ParticleResource : EntityBase
    {
        public PXDStaticVector* ParticleData
        {
            get
            {
                return (PXDStaticVector*)(Pointer + 0xF8);
            }
        }

        public bool TryFindParticleData(ParticleID particleID, out IntPtr result)
        {
            var array = ParticleData;

            if (array == null)
            {
                result = IntPtr.Zero;
                return false;
            }

            for(int i = 0; i< array->ElementSize; i++)
            {
                IntPtr dataPtr = array->ElementAt<IntPtr>(i);

                if(Marshal.ReadInt32(dataPtr + 0x20) == (int)particleID)
                {
                    result = dataPtr;
                    return true;
                }
            }

            result = IntPtr.Zero;
            return false;
        }
    }
}
