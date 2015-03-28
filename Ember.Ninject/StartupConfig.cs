namespace Ember.Ninject
{
    using System.Web.Http;

    /// <summary>
    /// The startup config.
    /// </summary>
    public class StartupConfig
    {
        private static HttpConfiguration config;

        /// <summary>
        /// Gets the config.
        /// </summary>
        public static HttpConfiguration Config
        {
            get
            {
                return config ?? (config = new HttpConfiguration());
            }
        }
    }
}