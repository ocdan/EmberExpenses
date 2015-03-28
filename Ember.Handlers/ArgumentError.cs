using System.Collections.Generic;

namespace Ember.Handlers
{
    /// <summary>
    /// The argument error.
    /// </summary>
    public class ArgumentError : IError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentError"/> class.
        /// </summary>
        public ArgumentError()
        {
            this.ErrorCode = ErrorCodes.Argument;
            this.Errors = new List<string> { "Argument error" };
        }

        public ArgumentError(IEnumerable<string> errors)
        {
            this.ErrorCode = ErrorCodes.Argument;
            this.Errors = errors;
        }

        public ArgumentError(string message)
            : this(new List<string> { message })
        {
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        public IEnumerable<string> Errors { get; private set; }
    }
}
