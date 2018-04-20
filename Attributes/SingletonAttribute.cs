using System;

namespace yaSingleton.Attributes {
    /// <summary>
    /// Internal attributed used to avoid Mono errors when using Reflection to get inherited generic types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class SingletonAttribute : Attribute { }
}