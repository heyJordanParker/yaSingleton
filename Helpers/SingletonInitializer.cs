using UnityEngine;

namespace yaSingleton.Helpers {
    /// <summary>
    /// Singleton initializer. Scans all assemblies for singletons and initializes them before the scene has loaded. Add any assemblies you'd want to ignore to the BuiltinAssemblies array.
    /// </summary>
    public static class SingletonInitializer {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons() {
            foreach(var singleton in BaseSingleton.AllSingletons) {
                singleton.CreateInstance();
            }
        }
    }
}