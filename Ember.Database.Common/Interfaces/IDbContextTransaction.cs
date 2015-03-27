namespace Ember.Database.Common.Interfaces
{
    using System;

    /// <summary>
    /// The DbContextTransaction interface.
    /// </summary>
    public interface IDbContextTransaction : IDisposable
    {
        /// <summary>
        /// The commit.
        /// </summary>
        void Commit();

        /// <summary>
        /// The rollback.
        /// </summary>
        void Rollback();
    }
}