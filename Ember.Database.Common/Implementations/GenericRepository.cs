namespace Ember.Database.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    using Ember.Database.Common.Interfaces;

    /// <summary>
    /// The repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The db set.
        /// </summary>
        private IDbSet<TEntity> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// </summary>
        public GenericRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public GenericRepository(IDbContext context)
        {
            this.SetContext(context);
        }

        /// <summary>
        /// The context.
        /// </summary>
        protected IDbContext Context { get; set; }

        /// <summary>
        /// The database set.
        /// </summary>
        protected IDbSet<TEntity> DbSet
        {
            get
            {
                return this.dbSet;
            }

            set
            {
                this.dbSet = value;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public virtual void Delete(object id)
        {
            var entityToDelete = this.dbSet.Find(id);
            this.Delete(entityToDelete);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity to delete.
        /// </param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.dbSet.Attach(entityToDelete);
            }

            this.dbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// The find by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        public virtual TEntity FindById(object id)
        {
            return this.dbSet.Find(id);
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
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            ICollection<Expression<Func<TEntity, object>>> includeProperties = null, 
            int? page = null, 
            int? pageSize = null)
        {
            IQueryable<TEntity> query = this.dbSet;
            return this.BuildQuery(query, filter, orderBy, includeProperties, page, pageSize);
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Insert(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this.Context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// The insert graph.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void InsertGraph(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        /// <summary>
        /// The query.
        /// </summary>
        /// <returns>
        /// The <see cref="RepositoryQuery{T}"/>.
        /// </returns>
        public virtual RepositoryQuery<TEntity> Query()
        {
            var repositoryGetFluentHelper = new RepositoryQuery<TEntity>(this);

            return repositoryGetFluentHelper;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public virtual void Update(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this.Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// The sql query.
        /// </summary>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <typeparam name="TElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="DbRawSqlQuery"/>.
        /// </returns>
        public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Context.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// The build query.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
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
        /// The <see cref="IQueryable{T}"/>.
        /// </returns>
        protected IQueryable<TEntity> BuildQuery(
            IQueryable<TEntity> query, 
            Expression<Func<TEntity, bool>> filter, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, 
            ICollection<Expression<Func<TEntity, object>>> includeProperties, 
            int? page, 
            int? pageSize)
        {
            var result = query;

            if (includeProperties != null)
            {
                includeProperties.ToList().ForEach(i => { result = result.Include(i); });
            }

            if (filter != null)
            {
                result = result.Where(filter);
            }

            if (orderBy != null)
            {
                result = orderBy(result);
            }

            if (page != null && pageSize != null)
            {
                result = result.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return result;
        }

        /// <summary>
        /// The set context.
        /// </summary>
        /// <param name="newContext">
        /// The new context.
        /// </param>
        protected void SetContext(IDbContext newContext)
        {
            this.Context = newContext;
            this.dbSet = newContext != null ? newContext.Set<TEntity>() : null;
        }
    }
}