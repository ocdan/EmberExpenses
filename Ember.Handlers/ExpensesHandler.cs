namespace Ember.Handlers
{
    using System.Collections.Generic;

    using Ember.Database.Common.Interfaces;
    using Ember.Models;

    public class ExpensesHandler
    {
        private readonly IUnitOfWork unitOfWork;

        public ExpensesHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public HandlerResult<IEnumerable<ExpenseLine>> GetAll()
        {
            var response = this.unitOfWork.Repository<ExpenseLine>().Get();

            return new HandlerResult<IEnumerable<ExpenseLine>>(response);
        }
    }
}
