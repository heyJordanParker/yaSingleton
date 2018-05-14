using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using yaSingleton.Helpers;
using Debug = UnityEngine.Debug;

namespace yaSingleton {
    /// <summary>
    /// Base class for singletons. Contains method stubs and the Create method. Use this to create custom Singleton flavors.
    /// If you're looking to create a singleton, inherit Singleton or LazySingleton.
    /// </summary>
    public abstract class BaseSingleton : ScriptableObject {

        internal abstract void CreateInstance();
        
        protected virtual void Initialize() {
            SingletonUpdater.RegisterSingleton(this, Deinitialize);
        }
        
        protected virtual void Deinitialize() { }
        
        #region UnityEvents
        
        public virtual void OnFixedUpdate() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnApplicationFocus() { }
        public virtual void OnApplicationPause() { }
        public virtual void OnApplicationQuit() { }
        public virtual void OnDrawGizmos() { }
        public virtual void OnPostRender() { }
        public virtual void OnPreCull() { }
        public virtual void OnPreRender() { }

        #endregion

        #region Coroutines

        /// <summary>
        ///   <para>Starts a coroutine.</para>
        /// </summary>
        /// <param name="routine"></param>
        protected Coroutine StartCoroutine(IEnumerator routine) {
            return SingletonUpdater.Updater.StartCoroutine(routine);
        }

        /// <summary>
        ///   <para>Starts a coroutine named methodName.</para>
        /// </summary>
        /// <param name="methodName"></param>
        protected Coroutine StartCoroutine(string methodName) {
            return SingletonUpdater.Updater.StartCoroutine(methodName);
        }

        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        protected void StopCoroutine(Coroutine routine) {
            SingletonUpdater.Updater.StopCoroutine(routine);
        }
        
        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        protected void StopCoroutine(IEnumerator routine) {
            SingletonUpdater.Updater.StopCoroutine(routine);
        }
        
        /// <summary>
        ///   <para>Stops the first coroutine named methodName, or the coroutine stored in routine running on this behaviour.</para>
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        /// <param name="routine">Name of the function in code, including coroutines.</param>
        protected void StopCoroutine(string methodName) {
            SingletonUpdater.Updater.StopCoroutine(methodName);
        }
        
        /// <summary>
        ///   <para>Stops all coroutines running on this behaviour.</para>
        /// </summary>
        protected void StopAllCoroutines() {
            SingletonUpdater.Updater.StopAllCoroutines();
        }
        
        #endregion

        private static BaseSingleton[] _allSingletons = null;
        
        public static BaseSingleton[] AllSingletons {
            get {
                if(_allSingletons == null) {
                    _allSingletons = Resources.LoadAll<BaseSingleton>(string.Empty).Where(
                            s => s.GetType().IsSubclassOf(typeof(BaseSingleton)))
                        .ToArray();
                }
                
                return _allSingletons;
            }
        }


        protected static T Create<T>() where T : BaseSingleton {
            var instance = AllSingletons.FirstOrDefault(s => s.GetType() == typeof(T)) as T;

            instance = instance ? instance : CreateInstance<T>();

            instance.Initialize();

            return instance;
        }
    }
}