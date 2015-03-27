namespace Ember.Database.Common.Implementations
{
    using Ember.Database.Common.Interfaces;

    public class StoredProcedureInvoker : IStoredProcedureInvoker
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        public StoredProcedureInvoker(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        public void Invoke(string execStatement, params object[] parameters)
        {
            var result = this.UnitOfWork.Context.Database.ExecuteSqlCommand(string.Format("Exec {0}", execStatement), parameters);
        }
    }
}
