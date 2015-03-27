using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ember.Database
{
    using System.Data.Entity;

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