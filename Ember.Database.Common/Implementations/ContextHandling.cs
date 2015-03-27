namespace Ember.Database.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;

    /// <summary>
    /// The context handling.
    /// </summary>
    public class ContextHandling
    {
        /// <summary>
        /// The _mapping cache.
        /// </summary>
        private static Dictionary<Type, EntitySetBase> mappingCache = new Dictionary<Type, EntitySetBase>();

        /// <summary>
        /// The context.
        /// </summary>
        private readonly object context;

        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database database;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextHandling"/> class.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public ContextHandling(Database db, object context)
        {
            this.database = db;
            this.context = context;
        }

        /// <summary>
        /// The soft delete.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        public void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();

            string tableName = this.GetTableName(entryEntityType);

            string primaryKeyName = this.GetPrimaryKeyName(entryEntityType);

            string deletequery = string.Format(
                "UPDATE {0} SET IsDeleted = 1 WHERE {1} = @id", tableName, primaryKeyName);

            this.database.ExecuteSqlCommand(deletequery, new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));

            // Marking it Unchanged prevents the hard delete

            // entry.State = EntityState.Unchanged;

            // So does setting it to Detached:

            // And that is what EF does when it deletes an item

            // http://msdn.microsoft.com/en-us/data/jj592676.aspx
            entry.State = EntityState.Detached;
        }

        /// <summary>
        /// The get entity set.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="EntitySetBase"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        private EntitySetBase GetEntitySet(Type type)
        {
            if (!mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this.context).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es =
                    octx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace)
                        .GetItems<EntityContainer>()
                        .SelectMany(c => c.BaseEntitySets.Where(e => e.Name == typeName))
                        .FirstOrDefault();

                if (es == null)
                {
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);
                }

                mappingCache.Add(type, es);
            }

            return mappingCache[type];
        }

        /// <summary>
        /// The get table name.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetTableName(Type type)
        {
            EntitySetBase es = this.GetEntitySet(type);

            return string.Format(
                "[{0}].[{1}]", es.MetadataProperties["Schema"].Value, es.MetadataProperties["Table"].Value);
        }

        /// <summary>
        /// The get primary key name.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = this.GetEntitySet(type);

            return es.ElementType.KeyMembers[0].Name;
        }
    }
}