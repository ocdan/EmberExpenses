namespace Ember.Database
{
    using System.Data.Entity;

    using Ember.Models;

    public class EmberContext : DbContext
    {
        public EmberContext()
            : base("name=EmberDbConnectionString")
        {
        }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<ExpenseLine> ExpenseLines { get; set; }

        public DbSet<CostCode> CostCodes { get; set; }

        public DbSet<StatementLine> StatementLines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here

            base.OnModelCreating(modelBuilder);
        }
    }
}
