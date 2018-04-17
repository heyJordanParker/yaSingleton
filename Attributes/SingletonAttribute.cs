using System;

namespace Elarion.Singleton.Attributes {
    /// <summary>
    /// Internal attributed used to avoid Mono errors when using Reflection to get inherited generic types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class SingletonAttribute : Attribute { }
}