using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public class ECSoldierInfo : EntityComponent
    {
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_HEALTH_GAUGE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECSoldierInfo_Getter_HealthGauge(IntPtr agent);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_NAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_ECSoldierInfo_Getter_Name(IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_HP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DELib_ECSoldierInfo_Getter_Hp(IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_SETTER_HP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECSoldierInfo_Setter_Hp(IntPtr inf, long value);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_MAX_HP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern long DELib_ECSoldierInfo_Getter_MaxHp(IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_SETTER_MAX_HP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECSoldierInfo_Setter_MaxHp(IntPtr inf, long value);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_GETTER_POWER_RATIO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DELib_ECSoldierInfo_Getter_PowerRatio(IntPtr inf);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_SETTER_POWER_RATIO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECSoldierInfo_Setter_PowerRatio(IntPtr inf, float value);

#if YK2
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_ATTACH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_ECSoldierInfo_Attach(IntPtr chara);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_CEC_SOLDIER_INFO_INITIALIZE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_ECSoldierInfo_Initialize(IntPtr chara, CharacterNPCSoldierPersonalDataID id);

        public long HP
        {
            get { return DELib_ECSoldierInfo_Getter_Hp(Pointer); }
            set { DELib_ECSoldierInfo_Setter_Hp(Pointer, value); }
        }

        public long MaxHP
        {
            get { return DELib_ECSoldierInfo_Getter_MaxHp(Pointer); }
            set { DELib_ECSoldierInfo_Setter_MaxHp(Pointer, value); }
        }

        public float PowerRatio
        {
            get { return DELib_ECSoldierInfo_Getter_PowerRatio(Pointer); }
            set { DELib_ECSoldierInfo_Setter_PowerRatio(Pointer, value); }
        }

        public static EntityComponentHandle<ECSoldierInfo> Attach(Character chara)
        {
            return DELib_ECSoldierInfo_Attach(chara.Pointer);
        }

        public void  Initialize(CharacterNPCSoldierPersonalDataID id)
        {
            DELib_ECSoldierInfo_Initialize(Pointer, id);
        }
#endif


        ///<summary>Get the UI health gauge component of the character.</summary>
        public EntityComponentHandle<UIEntityComponentEnemyLifeGauge> HealthGauge
        {
            get { return DELib_ECSoldierInfo_Getter_HealthGauge(Pointer); }
        }

        public string Name
        {
            get
            {
                return Marshal.PtrToStringUTF8(DELib_ECSoldierInfo_Getter_Name(Pointer));
            }
        }
    }
}
