using System;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public static class Item
    {
        ///<summary>Get the asset ID of this item.</summary>
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ITEM_GET_ASSET_ID", CallingConvention = CallingConvention.Cdecl)]
        public static extern AssetID GetAssetID(ItemID item);

        ///<summary>Get the name of this item.</summary>
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ITEM_GET_NAME", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr _GetName(ItemID item);

        ///<summary>Get the name of this item.</summary>
        public static string GetName(ItemID item)
        {
            var res = _GetName(item);
            return Marshal.PtrToStringUTF8(res);
        }
    }
}
