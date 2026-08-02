namespace DragonEngineLibrary
{
    public class DragonEngineMod
    {
        public virtual string ModPath
        {
            get; internal set;
        }


        /// <summary>
        /// A mod's entrypoint. This function will be called upon mod load.
        /// </summary>
        public virtual void OnModInit()
        {

        }


        /// <summary>
        /// This function will be called upon mod unload. It is recommended to dispose of all relevant resources and undo any patches or hooks here.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the operation was successful, otherwise <see langword="false"/>.</returns>
        /// <remarks><b>Important:</b> The mod will not be unloaded if this function returns <see langword="false"/>.</remarks>
        public virtual bool OnModUnload()
        {
            return false;
        }
    }
}
