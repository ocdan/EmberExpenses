using System.Web.Http;

namespace Ember.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http.Description;

    using Ember.Handlers;
    using Ember.Models;

    [RoutePrefix("api/staticdata")]
    public class ExpensesController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAirlines")]
        [ResponseType(typeof(IEnumerable<ExpenseLine>))]
        public IHttpActionResult GetExpenses()
        {
            var response = this.Using<ExpensesHandler>().GetAll();

            return this.Ok(response.Data);
        }
    }
}
