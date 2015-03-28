namespace Ember.Handlers
{
    using System.Collections.Generic;

    /// <summary>
    /// The Error interface.
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        int ErrorCode { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        IEnumerable<string> Errors { get; }
    }
}
