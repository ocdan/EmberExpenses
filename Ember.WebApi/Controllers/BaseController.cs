namespace Ember.WebApi.Controllers
{
    using System.Net.Http;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using Ember.Handlers;
    using Ember.Ninject.Helpers;
    using Ember.WebApi.Errors;

    /// <summary>
    /// The base controller.
    /// </summary>
    public abstract class BaseController : ApiController
    {
        /// <summary>
        /// The errors.
        /// </summary>
        private readonly Dictionary<int, HttpStatusCode> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        protected BaseController()
        {
            this.errors = new Dictionary<int, HttpStatusCode>();
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="ApiError"/>.
        /// </returns>
        protected ApiError Error<T>() where T : IError, new()
        {
            var errorInstance = new T();

            return this.Error(errorInstance);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <returns>
        /// The <see cref="ApiError"/>.
        /// </returns>
        protected ApiError Error(IError error)
        {
            ApiError errorInstance;
            if (this.errors.ContainsKey(error.ErrorCode))
            {
                errorInstance = new ApiError(this.errors[error.ErrorCode], error, this);
            }
            else
            {
                errorInstance = new ApiError(HttpStatusCode.BadRequest, error, this);
            }

            return errorInstance;
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="errorList">
        /// The error list.
        /// </param>
        /// <returns>
        /// The <see cref="ApiError"/>.
        /// </returns>
        protected ApiError Error(IEnumerable<string> errorList)
        {
            return new ApiError(HttpStatusCode.InternalServerError, new InternalServerError(errorList), this);
        }

        /// <summary>
        /// The get model error.
        /// </summary>
        /// <returns>
        /// The <see cref="ApiError"/>.
        /// </returns>
        protected ApiError GetModelError()
        {
            if (this.ModelState.IsValid)
            {
                return null;
            }

            return new ApiError(
                HttpStatusCode.BadRequest,
                new ArgumentError(this.ModelState.Values.First(v => v.Errors.Any()).Errors.Select(x => x.ErrorMessage)),
                this);
        }

        /// <summary>
        /// The not found.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <returns>
        /// The <see cref="ApiError"/>.
        /// </returns>
        protected ApiError NotFound(IError error)
        {
            return new ApiError(HttpStatusCode.NotFound, error, this);
        }

        /// <summary>
        /// Gets instance of type T from Ninject context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Using<T>() where T : class
        {
            return DependencyHelper.Using<T>();
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }
}