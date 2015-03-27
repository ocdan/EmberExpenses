namespace Ember.Database.Initialize
{
    /// <summary>
    /// The database setup.
    /// </summary>
    public class DatabaseSetup
    {
        /// <summary>
        /// Initialises the database.
        /// </summary>
        public static void Initialize()
        {
            using (var context = new EmberContext())
            {
                context.Database.Initialize(true);
            }
        }
    }
}