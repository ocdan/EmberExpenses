using System.Web.Http;

namespace Ember.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http.Description;

    using Ember.Handlers;
    using Ember.Models;

    [RoutePrefix("api/expenses")]
    public class ExpensesController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetExpenses")]
        [ResponseType(typeof(IEnumerable<ExpenseLine>))]
        public IHttpActionResult GetExpenses()
        {
            var response = this.Using<ExpensesHandler>().GetAll();

            return this.Ok(response.Data);
        }
    }
}
