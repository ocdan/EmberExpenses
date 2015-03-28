namespace Ember.WebApi.Errors
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Ember.Handlers;

    /// <summary>
    /// Custom Error
    /// </summary>
    public class ApiError : IHttpActionResult
    {
        /// <summary>
        /// The error code.
        /// </summary>
        private readonly int errorCode;

        /// <summary>
        /// The error message.
        /// </summary>
        private readonly string errorMessage;

        /// <summary>
        /// The request.
        /// </summary>
        private readonly HttpRequestMessage request;

        /// <summary>
        /// The status code.
        /// </summary>
        private readonly HttpStatusCode statusCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiError"/> class. 
        /// Constructor
        /// </summary>
        /// <param name="statusCode">
        /// The status Code.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="controller">
        /// The controller.
        /// </param>
        public ApiError(HttpStatusCode statusCode, IError error, ApiController controller)
        {
            this.statusCode = statusCode;
            this.errorCode = error.ErrorCode;
            this.errorMessage = string.Join(",", error.Errors);
            this.request = controller == null ? null : controller.Request;
        }

        /// <summary>
        /// The error code.
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return this.errorCode;
            }
        }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        /// <summary>
        /// The status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }

        /// <summary>
        /// The execute async.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.request.CreateResponse(this.StatusCode, new { this.ErrorCode, this.ErrorMessage });
            return Task.FromResult(response);
        }

        /// <summary>
        /// Execute synchronously
        /// </summary>
        /// <param name="context">
        /// </param>
        public void Execute(HttpActionContext context)
        {
            context.Response = context.Request.CreateResponse(
                this.StatusCode,
                new { this.ErrorCode, this.ErrorMessage });
        }

        /// <summary>
        /// Execute synchronously
        /// </summary>
        /// <param name="context">
        /// </param>
        public void Execute(HttpActionExecutedContext context)
        {
            context.Response = context.Request.CreateResponse(
                this.StatusCode,
                new { this.ErrorCode, this.ErrorMessage });
        }
    }
}
