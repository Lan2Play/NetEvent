using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NetEvent.Server.Data.Events
{
    /// <summary>
    /// Represents the result of an Event operation.
    /// </summary>
    public class EventResult
    {
        private static readonly EventResult _Success = new() { Succeeded = true };
        private readonly List<EventError> _Errors = new();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="EventError"/> instances containing errors
        /// that occurred during the Event operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="EventError"/> instances.</value>
        public IEnumerable<EventError> Errors => _Errors;

        /// <summary>
        /// Returns an <see cref="EventResult"/> indicating a successful Event operation.
        /// </summary>
        /// <returns>An <see cref="EventResult"/> indicating a successful operation.</returns>
        public static EventResult Success => _Success;

        /// <summary>
        /// Creates an <see cref="EventResult"/> indicating a failed Event operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="EventError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="EventResult"/> indicating a failed Event operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static EventResult Failed(params EventError[] errors)
        {
            var result = new EventResult { Succeeded = false };
            if (errors != null)
            {
                result._Errors.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="EventResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="EventResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Description).ToList()));
        }
    }
}
