using System.Linq;
using Elarion.Singleton.Helpers;
using UnityEngine;

namespace Elarion.Singleton {
    /// <summary>
    /// Base class for singletons. Contains method stubs and the Create method. Use this to create custom Singleton flavors.
    /// If you're looking to create a singleton, inherit Singleton or LazySingleton.
    /// </summary>
    public abstract class BaseSingleton : ScriptableObject {
        protected virtual void Initialize() {
            SingletonUpdater.RegisterSingleton(this, Deinitialize);
        }

        protected virtual void Deinitialize() { }

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
        public virtual void OnReset() { }

        protected static T Create<T>() where T : BaseSingleton {
            var instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

            instance = instance ? instance : CreateInstance<T>();

            instance.Initialize();

            return instance;
        }
    }
}