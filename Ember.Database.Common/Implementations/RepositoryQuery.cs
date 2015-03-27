namespace Ember.Database.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Ember.Database.Common.Interfaces;

    /// <summary>
    /// The repository query.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public sealed class RepositoryQuery<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The _include properties.
        /// </summary>
        private readonly List<Expression<Func<TEntity, object>>> includeProperties;

        /// <summary>
        /// The _repository.
        /// </summary>
        private readonly IRepository<TEntity> repository;

        /// <summary>
        /// The _filter.
        /// </summary>
        private Expression<Func<TEntity, bool>> filter;

        /// <summary>
        /// The order by Queryable.
        /// </summary>
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByQueryable;

        /// <summary>
        /// The _page.
        /// </summary>
        private int? page;

        /// <summary>
        /// The _page size.
        /// </summary>
        private int? pageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryQuery{TEntity}"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public RepositoryQuery(IRepository<TEntity> repository)
        {
            this.repository = repository;
            this.includeProperties = new List<Expression<Func<TEntity, object>>>();
        }

        /// <summary>
        /// The filter.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="RepositoryQuery{TEntity}"/>.
        /// </returns>
        public RepositoryQuery<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            this.filter = filter;
            return this;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{TEntity}"/>.
        /// </returns>
        public IEnumerable<TEntity> Get()
        {
            return this.repository.Get(
                this.filter, this.orderByQueryable, this.includeProperties, this.page, this.pageSize);
        }

        /// <summary>
        /// The get page.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalCount">
        /// The total count.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{TEntity}"/>.
        /// </returns>
        public IEnumerable<TEntity> GetPage(int page, int pageSize, out int totalCount)
        {
            this.page = page;
            this.pageSize = pageSize;
            totalCount = this.repository.Get(this.filter).Count();

            return this.repository.Get(
                this.filter, this.orderByQueryable, this.includeProperties, this.page, this.pageSize);
        }

        /// <summary>
        /// The include.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="RepositoryQuery{TEntity}"/>.
        /// </returns>
        public RepositoryQuery<TEntity> Include(Expression<Func<TEntity, object>> expression)
        {
            this.includeProperties.Add(expression);
            return this;
        }

        /// <summary>
        /// The order by.
        /// </summary>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <returns>
        /// The <see cref="RepositoryQuery{TEntity}"/>.
        /// </returns>
        public RepositoryQuery<TEntity> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            this.orderByQueryable = orderBy;
            return this;
        }
    }
}