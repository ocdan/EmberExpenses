namespace Ember.Database.Common.Implementations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using Ember.Database.Common.Interfaces;

    /// <summary>
    /// The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Static Fields

        /// <summary>
        /// The cache repositories.
        /// </summary>
        private static readonly Hashtable CacheRepositories = new Hashtable();

        #endregion

        #region Fields

        /// <summary>
        /// The _disposed.
        /// </summary>
        private bool disposed;

       /// <summary>
        /// The _repositories.
        /// </summary>
        private Hashtable repositories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public UnitOfWork(IDbContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The _context.
        /// </summary>
        public IDbContext Context { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The <see cref="IDbContextTransaction"/>.</returns>
        public IDbContextTransaction BeginTransaction()
        {
            return this.Context.BeginTransaction();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The repository.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="IRepository{T}"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (CacheRepositories.ContainsKey(type))
            {
                var repo = (IRepositoryWithContext)CacheRepositories[type];
                repo.DbContext = this.Context;
                return (IRepository<T>)repo;
            }

            if (this.repositories == null)
            {
                this.repositories = new Hashtable();
            }

            if (!this.repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(T)),
                    this.Context);

                this.repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)this.repositories[type];
        }

        /// <summary>
        /// Saves changes.
        /// </summary>
        public void Save()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="item">The entity.</param>
        public void Delete<T>(T item) where T : class
        {   
            if (item == null)
            {
                return;
            }

            this.Context.Set<T>().Remove(item);
        }

        /// <summary>
        /// Deletes entities.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="items">The entities.</param>
        public void Delete<T>(List<T> items) where T : class
        {
            if (items == null)
            {
                return;
            }

            var set = this.Context.Set<T>();

            items.ForEach(i => set.Remove(i));
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add cache repository.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <typeparam name="TEntity">
        /// </typeparam>
        protected static void AddCacheRepository<TEntity>(CacheRepository<TEntity> repository) where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!CacheRepositories.Contains(type))
            {
                CacheRepositories.Add(type, repository);
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    foreach (var repository in CacheRepositories)
                    {
                        var repositoryWithContext = repository as IRepositoryWithContext;
                        if (repositoryWithContext != null)
                        {
                            repositoryWithContext.DbContext = null;
                        }
                    }

                    this.Context.Dispose();
                }
            }

            this.disposed = true;
        }

        #endregion
    }
}