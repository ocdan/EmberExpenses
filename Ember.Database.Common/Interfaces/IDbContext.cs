namespace Ember.Database.Common.Interfaces
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// The DbContext interface.
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        Database Database { get; }

        /// <summary>
        /// The begin transaction.
        /// </summary>
        /// <returns>
        /// The <see cref="IDbContextTransaction"/>.
        /// </returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// The dispose.
        /// </summary>
        void Dispose();

        /// <summary>
        /// The entry.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// The <see cref="DbEntityEntry"/>.
        /// </returns>
        DbEntityEntry Entry(object o);

        /// <summary>
        /// The save changes.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int SaveChanges();

        /// <summary>
        /// The set.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IDbSet{T}"/>.
        /// </returns>
        IDbSet<T> Set<T>() where T : class;
    }
}