namespace Ember.WebApi
{
    using Ember.Ninject;

    /// <summary>
    /// The Ninject config.
    /// </summary>
    public static class NinjectConfig
    {
        /// <summary>
        /// Starts initialisation of the Ninject bindings.
        /// </summary>
        public static void Start()
        {
            NinjectWebCommon.Start();
        }

        /// <summary>
        /// Destroys the Ninject bindings.
        /// </summary>
        public static void Stop()
        {
            NinjectWebCommon.Stop();
        }
    }
}

