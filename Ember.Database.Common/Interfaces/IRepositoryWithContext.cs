namespace Ember.Database.Common.Interfaces
{
    public interface IRepositoryWithContext
    {
        /// <summary>
        /// Gets or sets the db context.
        /// </summary>
        IDbContext DbContext { get; set; }
    }
}