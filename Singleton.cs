using System;
using yaSingleton.Attributes;

namespace yaSingleton {
    /// <summary>
    /// Singleton class. It'll be initialized before the Awake method of all other MonoBehaviours.
    /// Inherit by passing the inherited type (e.g. class GameManager : Singleton&lt;GameManager&gt;)
    /// </summary>
    /// <typeparam name="TSingleton">The Inherited Singleton's Type</typeparam>
    [Singleton]
    [Serializable]
    public abstract class Singleton<TSingleton> : BaseSingleton where TSingleton : BaseSingleton {
        public static TSingleton Instance { get; private set; }

        // Called via Reflection from the SingletonInitializer
        // ReSharper disable once UnusedMember.Global
        [SingletonCreateInstance]
        protected static void CreateInstance() {
            if(Instance != null) {
                return;
            }

            Instance = Create<TSingleton>();
        }
    }
}