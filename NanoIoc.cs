//-----------------------------------------------------------------------
// <copyright file="NanoIoC.cs" company="Matt lacey Limited">
//     Copyright © 2012 Matt Lacey
// </copyright>
//-----------------------------------------------------------------------
namespace Mrlacey
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface used to mark-up classes that we want to inject into
    /// </summary>
    /// <typeparam name="T">
    ///    The type you want to be able to inject.
    ///    This will typically be an interface.
    /// </typeparam>
    public interface ISupportInjectionOf<T> where T : class
    {
    }

    /// <summary>
    /// Class that supports the configuration of the types we want to inject and what 
    /// </summary>
    public class NanoIocConfig
    {
        /// <summary>
        /// The way to register a dependency.
        /// </summary>
        public static NanoIocConfig RegisterDependency<T>(T value) where T : class
        {
            ISupportInjectionExtensions.RegisterDependency(value);

            return new NanoIocConfig();
        }

        /// <summary>
        /// The way to register subsequent dependencies in a fluent manner
        /// </summary>
        public NanoIocConfig And<T>(T value) where T : class
        {
            ISupportInjectionExtensions.RegisterDependency(value);

            return this;
        }
    }

    // ReSharper disable InconsistentNaming
    // Resharper disabled as it doesn't like classes with names beginning I but the "Extensions" naming convention suggests the name should be this
    public static class ISupportInjectionExtensions
    {
        /// <summary>
        /// Where we keep track of the dependencies that have been registered
        /// </summary>
        private static readonly Dictionary<Type, object> RegisteredDependencies = new Dictionary<Type, object>();

        /// <summary>
        /// How we resolve a dependency
        /// </summary>
        public static T GetDependency<T>(this ISupportInjectionOf<T> impl) where T : class
        {
            if (RegisteredDependencies.ContainsKey(typeof(T)))
            {
                return (T)RegisteredDependencies[typeof(T)];
            }

            System.Diagnostics.Debug.WriteLine("Unknown dependency requested: {0}", typeof(T).Name);

            // We could throw an error here but this should be just as clear that there's an issue.
            return null;
        }

        /// <summary>
        /// Internally, how we register the dependencies that were set up in config
        /// </summary>
        internal static void RegisterDependency<T>(T value) where T : class
        {
            if (RegisteredDependencies.ContainsKey(typeof(T)))
            {
                RegisteredDependencies[typeof(T)] = value;
            }
            else
            {
                RegisteredDependencies.Add(typeof(T), value);
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
