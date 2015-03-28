namespace Ember.Ninject.Helpers
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Common methods for controllers and API controllers.
    /// </summary>
    public static class DependencyHelper
    {
        /// <summary>
        /// Usings this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Unable to resolve type with service locator; type  + typeof(T).Name</exception>
        [DebuggerStepThrough]
        public static T Using<T>() where T : class
        {
            var resolver = StartupConfig.Config.DependencyResolver;
            var handler = resolver.GetService(typeof(T)) as T;

            if (handler == null)
            {
                throw new NullReferenceException("Unable to resolve type with service locator; type " + typeof(T).Name);
            }

            return handler;
        }
    }
}