namespace Ember.Database.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    using Ember.Database.Common.Implementations;

    /// <summary>
    /// The Repository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    /// </typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        void Delete(object id);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Delete(TEntity entity);

        /// <summary>
        /// The find by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="TEntity"/>.
        /// </returns>
        TEntity FindById(object id);

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
        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            ICollection<Expression<Func<TEntity, object>>> includeProperties = null, 
            int? page = null, 
            int? pageSize = null);

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Insert(TEntity entity);

        /// <summary>
        /// The insert graph.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void InsertGraph(TEntity entity);

        /// <summary>
        /// The query.
        /// </summary>
        /// <returns>
        /// The <see cref="RepositoryQuery"/>.
        /// </returns>
        RepositoryQuery<TEntity> Query();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Update(TEntity entity);

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
        DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters);
    }
}