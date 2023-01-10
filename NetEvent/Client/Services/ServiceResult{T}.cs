using System.Diagnostics.CodeAnalysis;

namespace NetEvent.Client.Services
{
    [ExcludeFromCodeCoverage(Justification = "Ignore UI Services")]
    public class ServiceResult<T> : ServiceResult
    {
        private ServiceResult(T? data, bool successful, string? messageKey) : base(successful, messageKey)
        {
            ResultData = data;
        }

        public static ServiceResult<T> Success(T data, string? messageKey = null) => new(data, true, messageKey);

        public static new ServiceResult<T> Error(string messageKey) => new(default, false, messageKey);

        public T? ResultData { get; }
    }
}
