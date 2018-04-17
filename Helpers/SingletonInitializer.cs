using System;
using System.Linq;
using System.Reflection;
using Elarion.Singleton.Attributes;
using UnityEngine;

namespace Elarion.Singleton.Helpers {
    /// <summary>
    /// Singleton initializer. Scans all assemblies for singletons and initializes them before the scene has loaded. Add any assemblies you'd want to ignore to the BuiltinAssemblies array.
    /// </summary>
    public static class SingletonInitializer {
        private static readonly string[] BuiltinAssemblies = {
            "Assembly-CSharp-Editor", "Assembly-CSharp-Editor-firstpass", "Boo.", "ExCSS.Unity", "Mono", "Mono.",
            "mscorlib", "nunit.", "System", "System.", "Unity.", "UnityEngine", "UnityEditor", "UnityScript"
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeSingletons() {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
                !BuiltinAssemblies.Any(builtinAssemblyName => assembly.FullName.StartsWith(builtinAssemblyName)));

            var singletons =
                from assembly in assemblies
                from type in assembly.GetTypes()
                where IsSingleton(type)
                select type;

            foreach(var singleton in singletons) {
                var method = singleton
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .FirstOrDefault(m =>
                        m.GetCustomAttributes(typeof(SingletonCreateInstanceAttribute), true).Length > 0);

                method.Invoke(null, null);
            }
        }

        public static bool IsSingleton(Type type) {
            return !type.IsAbstract && !type.IsGenericType &&
                type.GetCustomAttributes(typeof(SingletonAttribute), true).Length > 0;
        }
    }
}