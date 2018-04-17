using System;

namespace Elarion.Singleton.Attributes {
    /// <summary>
    /// Internal attributed used to find the CreateInstance class used to initialize singletons. Used to avoid having string-referenced methods in the code.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class SingletonCreateInstanceAttribute : Attribute { }
}