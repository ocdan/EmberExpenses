namespace Ember.WebApi.App_Start
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
#if !DEBUG
                NinjectWebCommon.Start(new List<Type> { typeof(UnhandledExceptionFilter) });
#else
            NinjectWebCommon.Start();
#endif
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

