using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public struct UIHandleBase
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_DEACTIVATE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_Deactivate(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PAUSE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_Pause(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_RELEASE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_Release(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VALUE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetValue(ulong handle, float value);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_ITEM_DISABLED", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetItemDisabled(ulong handle, int index, bool call_from_child);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_WIDTH", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetWidth(ulong handle, float width);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_HEIGHT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetHeight(ulong handle, float height);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_TEXT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetText(ulong handle, IntPtr text);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_FRAME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetFrame(ulong handle, float frame);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_TIME", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetTime(ulong handle, float time);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_TEXTURE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetTexture(ulong handle, uint texture);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_TEXTURE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetTexture(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VISIBILITY_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetVisibilityID(ulong handle, uint texture);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_VISIBILITY_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetVisibilityID(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_POSITION_V", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELib_UIHandleBase_GetPositionV(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_POSITION_V", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetPositionV(ulong handle, Vector4 vec);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PLAY_ANIMATION_SET", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_PlayAnimationSet(ulong handle, uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_PLAY_ANIMATION", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_PlayAnimation(ulong handle, uint id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetVisible(ulong handle, bool visible);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CONTROL_ID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetControlID(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CHILD_NUM", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELib_UIHandleBase_GetChildCount(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CHILD", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIHandleBase_GetChild(ulong handle, int index);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_TRANSITION_ANIMATION_SET_TO", CallingConvention = CallingConvention.Cdecl)]
        internal static extern ulong DELib_UIHandleBase_TransitionAnimationSetTo(ulong handle, uint scene_id, uint animation_set_data_id, uint focus_operation_id);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_MATERIAL_COLOR", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetMaterialColor(ulong handle, uint colorID);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_SET_MATERIAL_COLOR2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELib_UIHandleBase_SetMaterialColor2(ulong handle, uint color);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_GET_CONTROL_BASE", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELib_UIHandleBase_GetControlBase(ulong handle);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_UI_HANDLE_CBASE_IS_VISIBLE", CallingConvention = CallingConvention.Cdecl)]
        [return:MarshalAs(UnmanagedType.U1)]
        internal static extern bool DELib_UIHandleBase_IsVisible(ulong handle);

        public ulong Handle;


        public UIHandleBase(ulong handle)
        {
            Handle = handle;
        }

        public bool IsValid()
        {
            return Handle != 0 && DELib_UIHandleBase_GetControlBase(Handle) != IntPtr.Zero;
        }

        public override string ToString()
        {
            return Handle.ToString("X");
        }

        public UIControlBase GetControlBase()
        {
            return new UIControlBase() { Pointer = DELib_UIHandleBase_GetControlBase(Handle) };
        }

        public Vector4 GetPosition()
        {
            return DELib_UIHandleBase_GetPositionV(Handle);
        }

        public bool IsVisible()
        {
            return DELib_UIHandleBase_IsVisible(Handle);
        }

        public void SetPosition(Vector4 pos)
        {
            DELib_UIHandleBase_SetPositionV(Handle, pos);
        }

        public void SetValue(float value)
        {
            DELib_UIHandleBase_SetValue(Handle, value);
        }

        public void SetWidth(float width)
        {
            DELib_UIHandleBase_SetWidth(Handle, width);
        }

        public void SetHeight(float height)
        {
            DELib_UIHandleBase_SetHeight(Handle, height);
        }

        public void SetText(string text)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes((text ?? "") + "\0");
            IntPtr ptr = Marshal.AllocHGlobal(encoded.Length);
            Marshal.Copy(encoded, 0, ptr, encoded.Length);
            DELib_UIHandleBase_SetText(Handle, ptr);
            Marshal.FreeHGlobal(ptr);
        }

        public void SetFrame(float frame)
        {
            DELib_UIHandleBase_SetFrame(Handle, frame);   
        }

        public void SetTime(float frame)
        {
            DELib_UIHandleBase_SetTime(Handle, frame);
        }

        public void SetVisible(bool visible)
        {
            DELib_UIHandleBase_SetVisible(Handle, visible);
        }

        public void SetMaterialColorID(uint col)
        {
            DELib_UIHandleBase_SetMaterialColor(Handle, col);
        }

        public unsafe void SetMaterialColor(uint col)
        {
#if !YLAD
            uint* colo = (uint*)GetControlBase().Pointer + 636;
            *colo = col;
            return;
#endif

            DELib_UIHandleBase_SetMaterialColor2(Handle, col);
        }

        public void SetTexture(uint texture)
        {
            DELib_UIHandleBase_SetTexture(Handle, texture);
        }

        public uint GetTexture()
        {
            return DELib_UIHandleBase_GetTexture(Handle);
        }

        public uint GetVisibilityID()
        {
            return DELib_UIHandleBase_GetVisibilityID(Handle);
        }

        public void SetVisibilityID(uint id)
        {
            DELib_UIHandleBase_SetVisibilityID(Handle, id);
        }

        public UIControlID GetControlID()
        {
            return (UIControlID)DELib_UIHandleBase_GetControlID(Handle);
        }
        public uint GetChildCount()
        {
            return DELib_UIHandleBase_GetChildCount(Handle);
        }
        public UIHandleBase GetChild(int index)
        {
            return new UIHandleBase() { Handle = DELib_UIHandleBase_GetChild(Handle, index) };
        }

        ///<summary>Pause the UI.</summary>
        public void Pause()
        {
            DELib_UIHandleBase_Pause(Handle);
        }

        ///<summary>Deactivate the UI.</summary>
        public void Deactivate()
        {
            DELib_UIHandleBase_Deactivate(Handle);
        }

        ///<summary>Destroy the UI.</summary>
        public void Release()
        {
            DELib_UIHandleBase_Release(Handle);
        }

        public void SetItemDisabled(int index, bool callFromChild = false)
        {
            DELib_UIHandleBase_SetItemDisabled(Handle, index, callFromChild);
        }

        ///<summary>Play animation set on UI.</summary>
        public void PlayAnimationSet(uint id)
        {
            DELib_UIHandleBase_PlayAnimationSet(Handle, id);
        }

        public void PlayAnimation(uint id)
        {
            DELib_UIHandleBase_PlayAnimation(Handle, id);
        }

        public void TransitionAnimationSetTo(uint scene_id, uint animation_set_data_id, uint focus_operation_id)
        {
            DELib_UIHandleBase_TransitionAnimationSetTo(Handle, scene_id, animation_set_data_id, focus_operation_id);
        }
    }
}
