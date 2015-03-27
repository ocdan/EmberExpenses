namespace Ember.Database.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Ember.Database.Common.Interfaces;

    /// <summary>
    /// The cache repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class CacheRepository<TEntity> : GenericRepository<TEntity>, IRepositoryWithContext
        where TEntity : class
    {
        /// <summary>
        /// The db set.
        /// </summary>
        private ICollection<TEntity> dbSetCached;

        /// <summary>
        /// The modified.
        /// </summary>
        private bool modified;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public CacheRepository(IDbContext context)
        {
            this.dbSetCached = context.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        public IDbContext DbContext
        {
            get
            {
                return this.Context;
            }

            set
            {
                this.SetContext(value);
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="includeProperties">
        /// The include properties.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public override IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            ICollection<Expression<Func<TEntity, object>>> includeProperties = null, 
            int? page = null, 
            int? pageSize = null)
        {
            var query = this.dbSetCached.AsQueryable();
            var doRefresh = false;

            if (this.modified)
            {
                this.modified = false;
                doRefresh = true;
            }

            if (doRefresh)
            {
                this.dbSetCached = this.DbContext.Set<TEntity>().ToList();
                query = this.dbSetCached.AsQueryable();
            }

            return this.BuildQuery(query, filter, orderBy, includeProperties, page, pageSize);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void Insert(TEntity entity)
        {
            base.Insert(entity);
            this.modified = true;
        }

        /// <summary>
        /// The insert graph.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public override void InsertGraph(TEntity entity)
        {
            base.InsertGraph(entity);
            this.modified = true;            
        }

        public override void Update(TEntity entity)
        {
            base.Update(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
        }
    }
}