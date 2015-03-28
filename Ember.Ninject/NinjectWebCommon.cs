using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ember.WebApi.Tests")]
[assembly: InternalsVisibleTo("Ember.WebApi")]

namespace Ember.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Filters;

    using Ember.Database.Common.Implementations;
    using Ember.Database.Common.Interfaces;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using global::Ninject;

    using global::Ninject.Web.Common;

    /// <summary>
    /// The inject web common.
    /// </summary>
    public static class NinjectWebCommon
    {
        /// <summary>
        /// The boot start.
        /// </summary>
        internal static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        internal static IDependencyResolver ServciceLocator
        {
            get
            {
                if (Bootstrapper.Kernel == null) return null;
                return new NinjectDependencyResolver(Bootstrapper.Kernel);
            }
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start(IEnumerable<Type> filterTypes = null)
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(() => CreateKernel(filterTypes));
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel(IEnumerable<Type> filterTypes = null)
        {
            var kernel = new StandardKernel();

            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NinjectBindings"]))
            {
                kernel.Load(ConfigurationManager.AppSettings["NinjectBindings"]);
            }

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                var config = StartupConfig.Config;

                config.DependencyResolver = new NinjectDependencyResolver(kernel);

                if (filterTypes == null)
                {
                    return kernel;
                }

                foreach (var filterType in filterTypes)
                {
                    config.Filters.Add(
                        (ExceptionFilterAttribute)
                        config.DependencyResolver.GetService(filterType));
                }

                return kernel;
            }
            catch
            {
                kernel.Dispose();

                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">
        /// The kernel.
        /// </param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            kernel.Bind<IPrincipal>()
                .ToMethod(context => Thread.CurrentPrincipal)
                .InRequestScope();
        }
    }
}