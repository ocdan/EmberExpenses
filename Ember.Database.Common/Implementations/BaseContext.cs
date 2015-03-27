namespace Ember.Database.Common.Implementations
{
    using System.Data.Entity;

    /// <summary>
    /// The base context.
    /// </summary>
    /// <typeparam name="TContext">
    /// </typeparam>
    public class BaseContext<TContext> : DbContext
        where TContext : DbContext
    {
        /// <summary>
        ///     Initializes static members of the <see cref="BaseContext" /> class.
        /// </summary>
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseContext{TContext}" /> class.
        /// </summary>
        protected BaseContext()
            : base("KeyTravelDataBase")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}