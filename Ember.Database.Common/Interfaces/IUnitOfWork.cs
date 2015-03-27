namespace Ember.Database.Common.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        IDbContext Context { get; }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The <see cref="IDbContextTransaction"/>.</returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// Retrieves the specified repository.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The <see cref="IRepository{T}"/>.</returns>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Saves changes.
        /// </summary>
        void Save();

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="item">The entity.</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Deletes entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="items">The entities.</param>
        void Delete<T>(List<T> items) where T : class;
    }
}