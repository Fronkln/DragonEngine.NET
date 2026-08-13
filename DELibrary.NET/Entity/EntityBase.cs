using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DragonEngineLibrary
{
    public struct Transform
    {
        internal EntityBase _owner;

        ///<summary>The position of this entity.</summary>
        public Vector3 Position
        {
            get{ return _owner.GetPosCenter(); }
            set { _owner.SetPosCenter(value); }
        }

        ///<summary>The rotation of this entity.</summary>
        public Quaternion Orient
        {
            get
            {
                // Quaternion quat = new Quaternion();
                IntPtr orient = EntityBase.DELibrary_EntityBase_Getter_Orient(_owner.Pointer);

                if (orient != IntPtr.Zero)
                    return Marshal.PtrToStructure<Quaternion>(orient);
                else
                    return new Quaternion();
            }
            set
            {
                IntPtr ptr = value.ToIntPtr();
                EntityBase.DELibrary_EntityBase_Setter_Orient(_owner.Pointer, value.ToIntPtr());

                Marshal.FreeHGlobal(ptr);
            }
        }

        /// <summary> Forward direction of this entity.</summary>
        public Vector3 forwardDirection { get { return Orient * Vector3.forward; } }
        /// <summary> Up direction of this entity.</summary>
        public Vector3 upDirection { get { return Orient * Vector3.up; } }
        /// <summary> Right direction of this entity.</summary>
        public Vector3 rightDirection { get { return Orient * Vector3.right; } }
    }

    public unsafe class EntityBase : CTask
    {

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_RELEASE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_ReleaseEntity(IntPtr entity, uint level = 0, bool no_sweeper = false);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_ENTITY_UID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern EntityUID DELibrary_EntityBase_Getter_EntityUID(IntPtr entity);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_SCENEROOT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_EntityBase_Getter_SceneRoot(IntPtr entity);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_EntityBase_Getter_Orient(IntPtr entity);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_SET_ORIENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_Setter_Orient(IntPtr entity, IntPtr res);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_GLOBAL_ENTITY_FROM_UID", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_EntityBase_GetGlobalEntityFromUID(ulong uid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_GET_ONLY_ONE_ENTITY", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_EntityBase_GetOnlyOneEntity(ulong uid);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern Vector4 DELibrary_EntityBase_GetPosCenter(IntPtr entity);
        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_SET_POS_CENTER", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DELibrary_EntityBase_SetPosCenter(IntPtr entity, Vector4 pos);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_ENTITY_COMPONENT_MAP", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_EntityBase_Getter_EntityComponentMap(IntPtr entity);

        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_ENTITY_COMPONENT_MAP_ROOT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_EntityBase_Getter_EntityComponentMapRoot(IntPtr entity);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_PARENT", CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DELibrary_EntityBase_Getter_Parent(IntPtr entity);


        [DllImport("Y7Internal.dll", EntryPoint = "LIB_ENTITY_GETTER_CHILD_LIST", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DELibrary_EntityBase_Getter_Child_List(IntPtr entity);

        public Transform Transform;

        ///<summary>Entity component root of this entity.</summary>
        public EntityComponentMapRoot EntityComponentMapRoot
        {
            get
            {
                IntPtr address = DELibrary_EntityBase_Getter_EntityComponentMapRoot(Pointer);

                EntityComponentMapRoot componentMap = new EntityComponentMapRoot();
                componentMap.Pointer = address;

                return componentMap;
            }
        }

        ///<summary>Entity component map of this entity.</summary>
        public EntityComponentMapSync EntityComponentMap
        {
            get
            {
                IntPtr address = DELibrary_EntityBase_Getter_EntityComponentMap(Pointer);

                EntityComponentMapSync componentMap = new EntityComponentMapSync();
                componentMap.Pointer = address;

                return componentMap;
            }
        }

        ///<summary>Unique entity UID of this entity.</summary>
        public EntityUID EntityUID
        {
            get
            {
                return DELibrary_EntityBase_Getter_EntityUID(_objectAddress);
            }
        }


        /// <summary>Get the parent of this entity.</summary>
        public EntityHandle<EntityBase> Parent
        {
            get
            {
                return DELibrary_EntityBase_Getter_Parent(Pointer);
            }
        }

        /// <summary>
        /// Get the scene entity this entity is part of.
        /// </summary>
        public EntityHandle<SceneBase> Scene
        {
            get
            {
                return DELibrary_EntityBase_Getter_SceneRoot(_objectAddress);
            }
        }
        
        public PXDNodeList* ChildList
        {
            get
            {
                return (PXDNodeList*)DELibrary_EntityBase_Getter_Child_List(Pointer); 
            }
        }

        public EntityBase()
        {
            Transform._owner = this;
        }

        /// <summary>
        /// Destroy the entity.
        /// </summary>
        public void DestroyEntity(uint level = 0, bool no_sweeper = false)
        {
            DELibrary_EntityBase_ReleaseEntity(_objectAddress, level, no_sweeper);
        }

        /// <summary>
        /// Get the entity's center position.
        /// </summary>
        public Vector3 GetPosCenter()
        {
            return (Vector3)DELibrary_EntityBase_GetPosCenter(_objectAddress);
        }

        /// <summary>
        /// Set entity position.
        /// </summary>
        public void SetPosCenter(Vector4 position)
        {
            DELibrary_EntityBase_SetPosCenter(_objectAddress, position);
        }

        /// <summary>
        /// Returns a scene entity
        /// </summary>
        public virtual EntityHandle<EntityBase> GetSceneEntity(SceneEntity sceneEnt)
        {
            return Scene.Get().GetSceneEntity(sceneEnt);
        }

        /// <summary>
        /// Only use this if you know the type of scene entity you are accessing. Improper use will cause crashes
        /// </summary>
        /// <returns></returns>
        public EntityHandle<T> GetSceneEntity<T>(SceneEntity sceneEnt) where T : EntityBase, new()
        {
            //Implicit operator automatically converts to handle
            return GetSceneEntity(sceneEnt).UID;
        }

        ///<summary>Get an entity from UID.</summary>
        public static EntityHandle<EntityBase> GetGlobalEntityFromUID(EntityUID uid)
        {
            return DELibrary_EntityBase_GetGlobalEntityFromUID(uid.UID);
        }

        public static EntityHandle<EntityBase> GetGlobalEntityFromUID(ulong uid)
        {
            return DELibrary_EntityBase_GetGlobalEntityFromUID(uid);
        }

        public static EntityHandle<EntityBase> GetOnlyOneEntity(uint ID)
        {
            return DELibrary_EntityBase_GetOnlyOneEntity(ID);
        }

        public Matrix4x4 GetMatrix()
        {
            Matrix4x4 mtx = new Matrix4x4();
            mtx.Position = Transform.Position;
            mtx.ForwardDirection = Transform.forwardDirection;
            mtx.LeftDirection = -Transform.rightDirection;
            mtx.UpDirection = Transform.upDirection;

            return mtx;
        }

        public T GetComponent<T>(ECSlotID slot) where T : EntityComponent, new()
        {
            return EntityComponentMap.GetComponent<T>(slot);
        }
    }
}
